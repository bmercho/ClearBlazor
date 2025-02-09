using Microsoft.AspNetCore.Components;
using SkiaSharp;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Xml.Linq;
using System.Diagnostics.Metrics;

namespace ClearBlazor
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// By default a grid will occupy all of the available space given by its parent.
    /// In other words HorizontalAlignment and VerticalAlignment are both by default 'Stretch'.
    /// </summary>
    public partial class Grid : PanelBase, IBorder
    {
        /// <summary>
        /// Defines columns by a comma delimited string of column widths. 
        /// A column width consists of one to three values separated by colons. The seconds and third values are optional.
        /// The first value can be one of:
        ///    '*'    - a weighted proportion of available space.
        ///    'auto' - the minimum width of the content
        ///    value  - a pixel value for the width
        /// The second value is the minimum width in pixels
        /// The third value is the maximum width in pixels
        /// 
        /// eg *,2*,auto,200  - Four columns, the 3rd column auto sizes to content, the 4th column is 200px wide, and the remaining space
        /// is shared between columns 1 and 2 but column 2 is twice as wide as column 1.
        /// eg *:100:200,* - Two columns sharing available width equally except column 1 must be a minimum of 100px and a maximum of 200px. 
        /// So if the available width is 600px then column 1 will be 200px and column 2 400px.
        /// If the available width is 150px then column 1 will be 100px and column 2 50px.
        /// If the available width is 300px then column 1 will be 150px and column 2 150px.
        /// </summary>        
        [Parameter]
        public string Columns { get; set; } = "*";

        /// <summary>
        /// Defines rows by a comma delimited string of row heights which are similar to columns. 
        /// </summary>
        [Parameter]
        public string Rows { get; set; } = "*";

        /// <summary>
        /// The spacing in pixels between each column
        /// </summary>
        [Parameter]
        public double ColumnSpacing { get; set; } = 0;

        /// <summary>
        /// The spacing in pixels between each row
        /// </summary>
        [Parameter]
        public double RowSpacing { get; set; } = 0;

        private const double c_epsilon = 1e-5;                  //  used in fp calculations

        bool _sizeToContentH;
        bool _sizeToContentV;
        bool _hasStarCellsH;
        bool _hasStarCellsV;
        bool _hasGroup3CellsInAutoRows;

        CellCache[] _cellCachesCollection = Array.Empty<CellCache>(); //  backing store for logical children
        int _cellGroup1;                                //  index of the first cell in first cell group
        int _cellGroup2;                                //  index of the first cell in second cell group
        int _cellGroup3;                                //  index of the first cell in third cell group
        int _cellGroup4;
        DefinitionBase[] _definitionsH = Array.Empty<DefinitionBase>(); //  collection of column definitions used during calc
        DefinitionBase[] _definitionsV = Array.Empty<DefinitionBase>();                 //  collection of row definitions used during calc
        List<ColumnDefinition> _columnDefinitions = new List<ColumnDefinition>();
        List<RowDefinition> _rowDefinitions = new List<RowDefinition>();
        DefinitionBase[] _tempDefinitions;
        int[] _definitionIndices;

        //  temporary array used during layout for various purposes
        DefinitionBase[] TempDefinitions
        {
            get
            {
                int requiredLength = Math.Max(_definitionsH.Length, _definitionsV.Length) * 2;
                if (_tempDefinitions == null || _tempDefinitions.Length < requiredLength)
                    _tempDefinitions = new DefinitionBase[requiredLength];
                return (_tempDefinitions);
            }
        }

        /// <summary>
        /// Helper accessor to definition indices.
        /// </summary>
        private int[] DefinitionIndices
        {
            get
            {
                int requiredLength = Math.Max(Math.Max(_definitionsH.Length, _definitionsV.Length), 1) * 2;

                if (_definitionIndices == null || _definitionIndices.Length < requiredLength)
                    _definitionIndices = new int[requiredLength];

                return _definitionIndices;
            }
        }
        private const int c_layoutLoopMaxCount = 5;
        private static readonly IComparer s_spanPreferredDistributionOrderComparer = new SpanPreferredDistributionOrderComparer();
        private static readonly IComparer s_spanMaxDistributionOrderComparer = new SpanMaxDistributionOrderComparer();
        private static readonly IComparer s_minRatioComparer = new MinRatioComparer();
        private static readonly IComparer s_maxRatioComparer = new MaxRatioComparer();
        private static readonly IComparer s_starWeightComparer = new StarWeightComparer();

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        protected override void DoPaint(SKCanvas canvas)
        {
            base.DoPaint(canvas);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size resultSize = new Size(0, 0);

            _columnDefinitions = GetColumnDefinitions(Columns);
            _rowDefinitions = GetRowDefinitions(Rows);

            _sizeToContentH = double.IsPositiveInfinity(constraint.Width);
            _sizeToContentV = double.IsPositiveInfinity(constraint.Height);

            ValidateDefinitionsHStructure();
            ValidateDefinitionsLayout(_definitionsH, _sizeToContentH);

            ValidateDefinitionsVStructure();
            ValidateDefinitionsLayout(_definitionsV, _sizeToContentV);

            ValidateCells();

            Debug.Assert(_columnDefinitions.Count > 0 && _rowDefinitions.Count > 0);

            //  Grid classifies cells into four groups depending on
            //  the column / row type a cell belongs to (number corresponds to
            //  group number):
            //
            //                   Px      Auto     Star
            //               +--------+--------+--------+
            //               |        |        |        |
            //            Px |    1   |    1   |    3   |
            //               |        |        |        |
            //               +--------+--------+--------+
            //               |        |        |        |
            //          Auto |    1   |    1   |    3   |
            //               |        |        |        |
            //               +--------+--------+--------+
            //               |        |        |        |
            //          Star |    4   |    2   |    4   |
            //               |        |        |        |
            //               +--------+--------+--------+
            //
            //  The group number indicates the order in which cells are measured.
            //  Certain order is necessary to be able to dynamically resolve star
            //  columns / rows sizes which are used as input for measuring of
            //  the cells belonging to them.
            //
            //  However, there are cases when topology of a grid causes cyclical
            //  size dependences. For example:
            //
            //
            //                         column width="Auto"      column width="*"
            //                      +----------------------+----------------------+
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //  row height="Auto"   |                      |      cell 1 2        |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      +----------------------+----------------------+
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //  row height="*"      |       cell 2 1       |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      |                      |                      |
            //                      +----------------------+----------------------+
            //
            //  In order to accurately calculate constraint width for "cell 1 2"
            //  (which is the remaining of grid's available width and calculated
            //  value of Auto column), "cell 2 1" needs to be calculated first,
            //  as it contributes to the Auto column's calculated value.
            //  At the same time in order to accurately calculate constraint
            //  height for "cell 2 1", "cell 1 2" needs to be calcualted first,
            //  as it contributes to Auto row height, which is used in the
            //  computation of Star row resolved height.
            //
            //  to "break" this cyclical dependency we are making (arbitrary)
            //  decision to treat cells like "cell 2 1" as if they appear in Auto
            //  rows. And then recalculate them one more time when star row
            //  heights are resolved.
            //
            //  (Or more strictly) the code below implement the following logic:
            //
            //                       +---------+
            //                       |  enter  |
            //                       +---------+
            //                            |
            //                            V
            //                    +----------------+
            //                    | Measure Group1 |
            //                    +----------------+
            //                            |
            //                            V
            //                          / - \
            //                        /       \
            //                  Y   /    Can    \    N
            //            +--------|   Resolve   |-----------+
            //            |         \  StarsV?  /            |
            //            |           \       /              |
            //            |             \ - /                |
            //            V                                  V
            //    +----------------+                       / - \
            //    | Resolve StarsV |                     /       \
            //    +----------------+               Y   /    Can    \    N
            //            |                      +----|   Resolve   |------+
            //            V                      |     \  StarsU?  /       |
            //    +----------------+             |       \       /         |
            //    | Measure Group2 |             |         \ - /           |
            //    +----------------+             |                         V
            //            |                      |                 +-----------------+
            //            V                      |                 | Measure Group2' |
            //    +----------------+             |                 +-----------------+
            //    | Resolve StarsU |             |                         |
            //    +----------------+             V                         V
            //            |              +----------------+        +----------------+
            //            V              | Resolve StarsU |        | Resolve StarsU |
            //    +----------------+     +----------------+        +----------------+
            //    | Measure Group3 |             |                         |
            //    +----------------+             V                         V
            //            |              +----------------+        +----------------+
            //            |              | Measure Group3 |        | Measure Group3 |
            //            |              +----------------+        +----------------+
            //            |                      |                         |
            //            |                      V                         V
            //            |              +----------------+        +----------------+
            //            |              | Resolve StarsV |        | Resolve StarsV |
            //            |              +----------------+        +----------------+
            //            |                      |                         |
            //            |                      |                         V
            //            |                      |                +------------------+
            //            |                      |                | Measure Group2'' |
            //            |                      |                +------------------+
            //            |                      |                         |
            //            +----------------------+-------------------------+
            //                                   |
            //                                   V
            //                           +----------------+
            //                           | Measure Group4 |
            //                           +----------------+
            //                                   |
            //                                   V
            //                               +--------+
            //                               |  exit  |
            //                               +--------+
            //
            //  where:
            //  *   all [Measure GroupN] - regular children measure process -
            //      each cell is measured given constraint size as an input
            //      and each cell's desired size is accumulated on the
            //      corresponding column / row;
            //  *   [Measure Group2'] - is when each cell is measured with
            //      infinite height as a constraint and a cell's desired
            //      height is ignored;
            //  *   [Measure Groups''] - is when each cell is measured (second
            //      time during single Grid.MeasureOverride) regularly but its
            //      returned width is ignored;
            //
            //  This algorithm is believed to be as close to ideal as possible.
            //  It has the following drawbacks:
            //  *   cells belonging to Group2 can be called to measure twice;
            //  *   iff during second measure a cell belonging to Group2 returns
            //      desired width greater than desired width returned the first
            //      time, such a cell is going to be clipped, even though it
            //      appears in Auto column.
            //

            MeasureCellsGroup(_cellGroup1, constraint, false, false);

            //  after Group1 is measured,  only Group3 may have cells belonging to Auto rows.
            bool canResolveStarsV = !_hasGroup3CellsInAutoRows;

            if (canResolveStarsV)
            {
                if (_hasStarCellsV) { ResolveStar(_definitionsV, constraint.Height); }
                MeasureCellsGroup(_cellGroup2, constraint, false, false);
                if (_hasStarCellsH) { ResolveStar(_definitionsH, constraint.Width); }
                MeasureCellsGroup(_cellGroup3, constraint, false, false);
            }
            else
            {
                //  if at least one cell exists in Group2, it must be measured before
                //  StarsU can be resolved.
                bool canResolveStarsU = _cellGroup2 > _cellCachesCollection.Length;
                if (canResolveStarsU)
                {
                    if (_hasStarCellsH) { ResolveStar(_definitionsH, constraint.Width); }
                    MeasureCellsGroup(_cellGroup3, constraint, false, false);
                    if (_hasStarCellsV) { ResolveStar(_definitionsV, constraint.Height); }
                }
                else
                {
                    // This is a revision to the algorithm employed for the cyclic
                    // dependency case described above. We now repeatedly
                    // measure Group3 and Group2 until their sizes settle. We
                    // also use a count heuristic to break a loop in case of one.

                    bool hasDesiredSizeUChanged = false;
                    int cnt = 0;

                    // Cache Group2MinWidths & Group3MinHeights
                    double[] group2MinSizes = CacheMinSizes(_cellGroup2, false);
                    double[] group3MinSizes = CacheMinSizes(_cellGroup3, true);

                    MeasureCellsGroup(_cellGroup2, constraint, false, true);

                    do
                    {
                        if (hasDesiredSizeUChanged)
                        {
                            // Reset cached Group3Heights
                            ApplyCachedMinSizes(group3MinSizes, true);
                        }

                        if (_hasStarCellsH) { ResolveStar(_definitionsH, constraint.Width); }
                        MeasureCellsGroup(_cellGroup3, constraint, false, false);

                        // Reset cached Group2Widths
                        ApplyCachedMinSizes(group2MinSizes, false);

                        if (_hasStarCellsV) { ResolveStar(_definitionsV, constraint.Height); }
                        MeasureCellsGroup(_cellGroup2, constraint, cnt == c_layoutLoopMaxCount, false, out hasDesiredSizeUChanged);
                    }
                    while (hasDesiredSizeUChanged && ++cnt <= c_layoutLoopMaxCount);
                }
            }

            MeasureCellsGroup(_cellGroup4, constraint, false, false);

            resultSize = new Size(
                            CalculateDesiredSize(_definitionsH),
                            CalculateDesiredSize(_definitionsV));
            return resultSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize,
                                                double offsetHeight,
                                                double offsetWidth)
        {
            try
            {
                SetFinalSize(_definitionsH, arrangeSize.Width, true);
                SetFinalSize(_definitionsV, arrangeSize.Height, false);
                for (int currentCell = 0; currentCell < _cellCachesCollection.Length; ++currentCell)
                {
                    var cell = Children[currentCell];
                    if (cell == null)
                    {
                        continue;
                    }

                    int columnIndex = _cellCachesCollection[currentCell].ColumnIndex;
                    int rowIndex = _cellCachesCollection[currentCell].RowIndex;
                    int columnSpan = _cellCachesCollection[currentCell].ColumnSpan;
                    int rowSpan = _cellCachesCollection[currentCell].RowSpan;

                    Rect cellRect = new Rect(
                        columnIndex == 0 ? 0.0 : _definitionsH[columnIndex].FinalOffset,
                        rowIndex == 0 ? 0.0 : _definitionsV[rowIndex].FinalOffset,
                        GetFinalSizeForRange(_definitionsH, columnIndex, columnSpan),
                        GetFinalSizeForRange(_definitionsV, rowIndex, rowSpan));

                    cellRect.Top += offsetHeight;
                    cellRect.Left += offsetWidth;

                    cell.Arrange(cellRect);
                }
            }
            finally
            {
                SetValid();
            }
            return (arrangeSize);
        }

        /// <summary>
        /// Calculates and sets final size for all definitions in the given array.
        /// </summary>
        /// <param name="definitions">Array of definitions to process.</param>
        /// <param name="finalSize">Final size to lay out to.</param>
        /// <param name="columns">True if sizing row definitions, false for columns</param>
        private void SetFinalSize(
            DefinitionBase[] definitions,
            double finalSize,
            bool columns)
        {
            //if (FrameworkAppContextSwitches.GridStarDefinitionsCanExceedAvailableSpace)
            // {
            //     SetFinalSizeLegacy(definitions, finalSize, columns);
            // }
            // else
            // {
            SetFinalSizeMaxDiscrepancy(definitions, finalSize, columns);
            // }
        }
        // new implementation, as of 4.7.  This incorporates the same algorithm
        // as in ResolveStarMaxDiscrepancy.  It differs in the same way that SetFinalSizeLegacy
        // differs from ResolveStarLegacy, namely (a) leaves results in def.SizeCache
        // instead of def.MeasureSize, (b) implements LayoutRounding if requested,
        // (c) stores intermediate results differently.
        // The LayoutRounding logic is improved:
        // 1. Use pre-rounded values during proportional allocation.  This avoids the
        //      same kind of problems arising from interaction with min/max that
        //      motivated the new algorithm in the first place.
        // 2. Use correct "nudge" amount when distributing roundoff space.   This
        //      comes into play at high DPI - greater than 134.
        // 3. Applies rounding only to real pixel values (not to ratios)
        private void SetFinalSizeMaxDiscrepancy(
            DefinitionBase[] definitions,
            double finalSize,
            bool columns)
        {
            int defCount = definitions.Length;
            int[] definitionIndices = DefinitionIndices;
            int minCount = 0, maxCount = 0;
            double takenSize = 0.0;
            double totalStarWeight = 0.0;
            int starCount = 0;      // number of unresolved *-definitions
            double scale = 1.0;   // scale factor applied to each *-weight;  negative means "Infinity is present"

            // Phase 1.  Determine the maximum *-weight and prepare to adjust *-weights
            double maxStar = 0.0;
            for (int i = 0; i < defCount; ++i)
            {
                DefinitionBase def = definitions[i];

                if (def.UserSize.IsStar)
                {
                    ++starCount;
                    def.MeasureSize = 1.0;  // meaning "not yet resolved in phase 3"
                    if (def.UserSize.Value > maxStar)
                    {
                        maxStar = def.UserSize.Value;
                    }
                }
            }

            if (Double.IsPositiveInfinity(maxStar))
            {
                // negative scale means one or more of the weights was Infinity
                scale = -1.0;
            }
            else if (starCount > 0)
            {
                // if maxStar * starCount > Double.Max, summing all the weights could cause
                // floating-point overflow.  To avoid that, scale the weights by a factor to keep
                // the sum within limits.  Choose a power of 2, to preserve precision.
                double power = Math.Floor(Math.Log(Double.MaxValue / maxStar / starCount, 2.0));
                if (power < 0.0)
                {
                    scale = Math.Pow(2.0, power - 4.0); // -4 is just for paranoia
                }
            }


            // normally Phases 2 and 3 execute only once.  But certain unusual combinations of weights
            // and constraints can defeat the algorithm, in which case we repeat Phases 2 and 3.
            // More explanation below...
            for (bool runPhase2and3 = true; runPhase2and3;)
            {
                // Phase 2.   Compute total *-weight W and available space S.
                // For *-items that have Min or Max constraints, compute the ratios used to decide
                // whether proportional space is too big or too small and add the item to the
                // corresponding list.  (The "min" list is in the first half of definitionIndices,
                // the "max" list in the second half.  DefinitionIndices has capacity at least
                // 2*defCount, so there's room for both lists.)
                totalStarWeight = 0.0;
                takenSize = 0.0;
                minCount = maxCount = 0;

                for (int i = 0; i < defCount; ++i)
                {
                    DefinitionBase def = definitions[i];

                    if (def.UserSize.IsStar)
                    {
                        if (def.MeasureSize < 0.0)
                        {
                            takenSize += -def.MeasureSize;  // already resolved
                        }
                        else
                        {
                            double starWeight = StarWeight(def, scale);
                            totalStarWeight += starWeight;

                            if (def.MinSizeForArrange > 0.0)
                            {
                                // store ratio w/min in MeasureSize (for now)
                                definitionIndices[minCount++] = i;
                                def.MeasureSize = starWeight / def.MinSizeForArrange;
                            }

                            double effectiveMaxSize = Math.Max(def.MinSizeForArrange, def.UserMaxSize);
                            if (!Double.IsPositiveInfinity(effectiveMaxSize))
                            {
                                // store ratio w/max in SizeCache (for now)
                                definitionIndices[defCount + maxCount++] = i;
                                def.SizeCache = starWeight / effectiveMaxSize;
                            }
                        }
                    }
                    else
                    {
                        double userSize = 0;

                        switch (def.UserSize.GridUnitType)
                        {
                            case (GridUnitType.Pixel):
                                userSize = def.UserSize.Value;
                                break;

                            case (GridUnitType.Auto):
                                userSize = def.MinSizeForArrange;
                                break;
                        }

                        double userMaxSize;

                        //if (def.IsShared)
                        //{
                        //    //  overriding userMaxSize effectively prevents squishy-ness.
                        //    //  this is a "solution" to avoid shared definitions from been sized to
                        //    //  different final size at arrange time, if / when different grids receive
                        //    //  different final sizes.
                        //    userMaxSize = userSize;
                        //}
                        //else
                        //{
                            userMaxSize = def.UserMaxSize;
                        //}

                        def.SizeCache = Math.Max(def.MinSizeForArrange, Math.Min(userSize, userMaxSize));
                        takenSize += def.SizeCache;
                    }
                }

                // Phase 3.  Resolve *-items whose proportional sizes are too big or too small.
                int minCountPhase2 = minCount, maxCountPhase2 = maxCount;
                double takenStarWeight = 0.0;
                double remainingAvailableSize = finalSize - takenSize;
                double remainingStarWeight = totalStarWeight - takenStarWeight;

                MinRatioIndexComparer minRatioIndexComparer = new MinRatioIndexComparer(definitions);
                Array.Sort(definitionIndices, 0, minCount, minRatioIndexComparer);
                MaxRatioIndexComparer maxRatioIndexComparer = new MaxRatioIndexComparer(definitions);
                Array.Sort(definitionIndices, defCount, maxCount, maxRatioIndexComparer);

                while (minCount + maxCount > 0 && remainingAvailableSize > 0.0)
                {
                    // the calculation
                    //            remainingStarWeight = totalStarWeight - takenStarWeight
                    // is subject to catastrophic cancellation if the two terms are nearly equal,
                    // which leads to meaningless results.   Check for that, and recompute from
                    // the remaining definitions.   [This leads to quadratic behavior in really
                    // pathological cases - but they'd never arise in practice.]
                    const double starFactor = 1.0 / 256.0;      // lose more than 8 bits of precision -> recalculate
                    if (remainingStarWeight < totalStarWeight * starFactor)
                    {
                        takenStarWeight = 0.0;
                        totalStarWeight = 0.0;

                        for (int i = 0; i < defCount; ++i)
                        {
                            DefinitionBase def = definitions[i];
                            if (def.UserSize.IsStar && def.MeasureSize > 0.0)
                            {
                                totalStarWeight += StarWeight(def, scale);
                            }
                        }

                        remainingStarWeight = totalStarWeight - takenStarWeight;
                    }

                    double minRatio = (minCount > 0) ? definitions[definitionIndices[minCount - 1]].MeasureSize : Double.PositiveInfinity;
                    double maxRatio = (maxCount > 0) ? definitions[definitionIndices[defCount + maxCount - 1]].SizeCache : -1.0;

                    // choose the def with larger ratio to the current proportion ("max discrepancy")
                    double proportion = remainingStarWeight / remainingAvailableSize;
                    bool? chooseMin = Choose(minRatio, maxRatio, proportion);

                    // if no def was chosen, advance to phase 4;  the current proportion doesn't
                    // conflict with any min or max values.
                    if (!(chooseMin.HasValue))
                    {
                        break;
                    }

                    // get the chosen definition and its resolved size
                    int resolvedIndex;
                    DefinitionBase resolvedDef;
                    double resolvedSize;
                    if (chooseMin == true)
                    {
                        resolvedIndex = definitionIndices[minCount - 1];
                        resolvedDef = definitions[resolvedIndex];
                        resolvedSize = resolvedDef.MinSizeForArrange;
                        --minCount;
                    }
                    else
                    {
                        resolvedIndex = definitionIndices[defCount + maxCount - 1];
                        resolvedDef = definitions[resolvedIndex];
                        resolvedSize = Math.Max(resolvedDef.MinSizeForArrange, resolvedDef.UserMaxSize);
                        --maxCount;
                    }

                    // resolve the chosen def, deduct its contributions from W and S.
                    // Defs resolved in phase 3 are marked by storing the negative of their resolved
                    // size in MeasureSize, to distinguish them from a pending def.
                    takenSize += resolvedSize;
                    resolvedDef.MeasureSize = -resolvedSize;
                    takenStarWeight += StarWeight(resolvedDef, scale);
                    --starCount;

                    remainingAvailableSize = finalSize - takenSize;
                    remainingStarWeight = totalStarWeight - takenStarWeight;

                    // advance to the next candidate defs, removing ones that have been resolved.
                    // Both counts are advanced, as a def might appear in both lists.
                    while (minCount > 0 && definitions[definitionIndices[minCount - 1]].MeasureSize < 0.0)
                    {
                        --minCount;
                        definitionIndices[minCount] = -1;
                    }
                    while (maxCount > 0 && definitions[definitionIndices[defCount + maxCount - 1]].MeasureSize < 0.0)
                    {
                        --maxCount;
                        definitionIndices[defCount + maxCount] = -1;
                    }
                }

                // decide whether to run Phase2 and Phase3 again.  There are 3 cases:
                // 1. There is space available, and *-defs remaining.  This is the
                //      normal case - move on to Phase 4 to allocate the remaining
                //      space proportionally to the remaining *-defs.
                // 2. There is space available, but no *-defs.  This implies at least one
                //      def was resolved as 'max', taking less space than its proportion.
                //      If there are also 'min' defs, reconsider them - we can give
                //      them more space.   If not, all the *-defs are 'max', so there's
                //      no way to use all the available space.
                // 3. We allocated too much space.   This implies at least one def was
                //      resolved as 'min'.  If there are also 'max' defs, reconsider
                //      them, otherwise the over-allocation is an inevitable consequence
                //      of the given min constraints.
                // Note that if we return to Phase2, at least one *-def will have been
                // resolved.  This guarantees we don't run Phase2+3 infinitely often.
                runPhase2and3 = false;
                if (starCount == 0 && takenSize < finalSize)
                {
                    // if no *-defs remain and we haven't allocated all the space, reconsider the defs
                    // resolved as 'min'.   Their allocation can be increased to make up the gap.
                    for (int i = minCount; i < minCountPhase2; ++i)
                    {
                        if (definitionIndices[i] >= 0)
                        {
                            DefinitionBase def = definitions[definitionIndices[i]];
                            def.MeasureSize = 1.0;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;       // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }

                if (takenSize > finalSize)
                {
                    // if we've allocated too much space, reconsider the defs
                    // resolved as 'max'.   Their allocation can be decreased to make up the gap.
                    for (int i = maxCount; i < maxCountPhase2; ++i)
                    {
                        if (definitionIndices[defCount + i] >= 0)
                        {
                            DefinitionBase def = definitions[definitionIndices[defCount + i]];
                            def.MeasureSize = 1.0;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;    // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }
            }

            // Phase 4.  Resolve the remaining defs proportionally.
            starCount = 0;
            for (int i = 0; i < defCount; ++i)
            {
                DefinitionBase def = definitions[i];

                if (def.UserSize.IsStar)
                {
                    if (def.MeasureSize < 0.0)
                    {
                        // this def was resolved in phase 3 - fix up its size
                        def.SizeCache = -def.MeasureSize;
                    }
                    else
                    {
                        // this def needs resolution, add it to the list, sorted by *-weight
                        definitionIndices[starCount++] = i;
                        def.MeasureSize = StarWeight(def, scale);
                    }
                }
            }

            if (starCount > 0)
            {
                StarWeightIndexComparer starWeightIndexComparer = new StarWeightIndexComparer(definitions);
                Array.Sort(definitionIndices, 0, starCount, starWeightIndexComparer);

                // compute the partial sums of *-weight, in increasing order of weight
                // for minimal loss of precision.
                totalStarWeight = 0.0;
                for (int i = 0; i < starCount; ++i)
                {
                    DefinitionBase def = definitions[definitionIndices[i]];
                    totalStarWeight += def.MeasureSize;
                    def.SizeCache = totalStarWeight;
                }

                // resolve the defs, in decreasing order of weight.
                for (int i = starCount - 1; i >= 0; --i)
                {
                    DefinitionBase def = definitions[definitionIndices[i]];
                    double resolvedSize = (def.MeasureSize > 0.0) ? Math.Max(finalSize - takenSize, 0.0) * (def.MeasureSize / def.SizeCache) : 0.0;

                    // min and max should have no effect by now, but just in case...
                    resolvedSize = Math.Min(resolvedSize, def.UserMaxSize);
                    resolvedSize = Math.Max(def.MinSizeForArrange, resolvedSize);

                    // Use the raw (unrounded) sizes to update takenSize, so that
                    // proportions are computed in the same terms as in phase 3;
                    // this avoids errors arising from min/max constraints.
                    takenSize += resolvedSize;
                    def.SizeCache = resolvedSize;
                }
            }


            // Phase 6.  Compute final offsets
            definitions[0].FinalOffset = 0.0;
            for (int i = 0; i < definitions.Length; ++i)
            {
                definitions[(i + 1) % definitions.Length].FinalOffset =
                    definitions[i].FinalOffset + definitions[i].SizeCache;
            }
        }

        /// <summary>
        /// Calculates final (aka arrange) size for given range.
        /// </summary>
        /// <param name="definitions">Array of definitions to process.</param>
        /// <param name="start">Start of the range.</param>
        /// <param name="count">Number of items in the range.</param>
        /// <returns>Final size.</returns>
        private double GetFinalSizeForRange(
            DefinitionBase[] definitions,
            int start,
            int count)
        {
            double size = 0;
            int i = start + count - 1;

            do
            {
                size += definitions[i].SizeCache;
            } while (--i >= start);

            return (size);
        }

        /// <summary>
        /// Clears dirty state for the grid and its columns / rows
        /// </summary>
        private void SetValid()
        {
            if (_tempDefinitions != null)
            {
                //  TempDefinitions has to be cleared to avoid "memory leaks"
                Array.Clear(_tempDefinitions, 0, Math.Max(_definitionsH.Length, _definitionsV.Length));
                _tempDefinitions = null;
            }
        }


        /// <summary>
        /// Initializes DefinitionsU member either to user supplied ColumnDefinitions collection
        /// or to a default single element collection. DefinitionsU gets trimmed to size.
        /// </summary>
        /// <remarks>
        /// This is one of two methods, where ColumnDefinitions and DefinitionsU are directly accessed.
        /// All the rest measure / arrange / render code must use DefinitionsU.
        /// </remarks>
        private void ValidateDefinitionsHStructure()
        {
            _definitionsH = new DefinitionBase[_columnDefinitions.Count];
            for (int i = 0; i < _columnDefinitions.Count; i++)
            {
                _definitionsH[i] = new ColumnDefinition(_columnDefinitions[i].Width);
            }
            Debug.Assert(_definitionsH != null && _definitionsH.Length > 0);
        }

        /// <summary>
        /// Initializes DefinitionsV member either to user supplied RowDefinitions collection
        /// or to a default single element collection. DefinitionsV gets trimmed to size.
        /// </summary>
        /// <remarks>
        /// This is one of two methods, where RowDefinitions and DefinitionsV are directly accessed.
        /// All the rest measure / arrange / render code must use DefinitionsV.
        /// </remarks>
        private void ValidateDefinitionsVStructure()
        {
            _definitionsV = new DefinitionBase[_rowDefinitions.Count];
            for (int i = 0; i < _rowDefinitions.Count; i++)
            {
                _definitionsV[i] = new RowDefinition(_rowDefinitions[i].Height);
            }
            Debug.Assert(_definitionsV != null && _definitionsV.Length > 0);
        }

        /// <summary>
        /// Validates layout time size type information on given array of definitions.
        /// Sets MinSize and MeasureSizes.
        /// </summary>
        /// <param name="definitions">Array of definitions to update.</param>
        /// <param name="treatStarAsAuto">if "true" then star definitions are treated as Auto.</param>
        private void ValidateDefinitionsLayout(
            DefinitionBase[] definitions,
            bool treatStarAsAuto)
        {
            for (int i = 0; i < definitions.Length; ++i)
            {
                double userMinSize = definitions[i].UserMinSize;
                double userMaxSize = definitions[i].UserMaxSize;
                double userSize = 0;

                switch (definitions[i].UserSize.GridUnitType)
                {
                    case (GridUnitType.Pixel):
                        definitions[i].SizeType = GridUnitType.Pixel;
                        userSize = definitions[i].UserSize.Value;
                        // this was brought with NewLayout and defeats squishy behavior
                        userMinSize = Math.Max(userMinSize, Math.Min(userSize, userMaxSize));
                        break;
                    case (GridUnitType.Auto):
                        definitions[i].SizeType = GridUnitType.Auto;
                        userSize = double.PositiveInfinity;
                        break;
                    case (GridUnitType.Star):
                        if (treatStarAsAuto)
                        {
                            definitions[i].SizeType = GridUnitType.Auto;
                            userSize = double.PositiveInfinity;
                        }
                        else
                        {
                            definitions[i].SizeType = GridUnitType.Star;
                            userSize = double.PositiveInfinity;
                        }
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                definitions[i].UpdateMinSize(userMinSize);
                definitions[i].MeasureSize = Math.Max(userMinSize, Math.Min(userSize, userMaxSize));
            }
        }

        /// <summary>
        /// Lays out cells according to rows and columns, and creates lookup grids.
        /// </summary>
        private void ValidateCells()
        {
            _cellCachesCollection = new CellCache[Children.Count];
            _cellGroup1 = int.MaxValue;
            _cellGroup2 = int.MaxValue;
            _cellGroup3 = int.MaxValue;
            _cellGroup4 = int.MaxValue;

            bool hasStarCellsU = false;
            bool hasStarCellsV = false;
            bool hasGroup3CellsInAutoRows = false;

            for (int i = _cellCachesCollection.Length - 1; i >= 0; --i)
            {
                ClearComponentBase child = Children[i];
                if (child == null)
                {
                    continue;
                }

                CellCache cell = new CellCache
                {
                    //
                    //  read and cache child positioning properties
                    //

                    //  read indices from the corresponding properties
                    //      clamp to value < number_of_columns
                    //      column >= 0 is guaranteed by property value validation callback
                    ColumnIndex = Math.Min(child.Column, _definitionsH.Length - 1),
                    //      clamp to value < number_of_rows
                    //      row >= 0 is guaranteed by property value validation callback
                    RowIndex = Math.Min(child.Row, _definitionsV.Length - 1)
                };

                //  read span properties
                //      clamp to not exceed beyond right side of the grid
                //      column_span > 0 is guaranteed by property value validation callback
                cell.ColumnSpan = Math.Min(child.ColumnSpan, _definitionsH.Length - cell.ColumnIndex);

                //      clamp to not exceed beyond bottom side of the grid
                //      row_span > 0 is guaranteed by property value validation callback
                cell.RowSpan = Math.Min(child.RowSpan, _definitionsV.Length - cell.RowIndex);

                Debug.Assert(0 <= cell.ColumnIndex && cell.ColumnIndex < _definitionsH.Length);
                Debug.Assert(0 <= cell.RowIndex && cell.RowIndex < _definitionsV.Length);

                //
                //  calculate and cache length types for the child
                //

                cell.SizeTypeH = GetHLengthTypeForRange(_definitionsH, cell.ColumnIndex, cell.ColumnSpan);
                cell.SizeTypeV = GetVLengthTypeForRange(_definitionsV, cell.RowIndex, cell.RowSpan);

                hasStarCellsU |= cell.IsStarH;
                hasStarCellsV |= cell.IsStarV;

                //
                //  distribute cells into four groups.
                //

                if (!cell.IsStarV)
                {
                    if (!cell.IsStarH)
                    {
                        cell.Next = _cellGroup1;
                        _cellGroup1 = i;
                    }
                    else
                    {
                        cell.Next = _cellGroup3;
                        _cellGroup3 = i;

                        //  remember if this cell belongs to auto row
                        hasGroup3CellsInAutoRows |= cell.IsAutoV;
                    }
                }
                else
                {
                    if (cell.IsAutoH
                        //  note below: if spans through Star column it is NOT Auto
                        && !cell.IsStarH)
                    {
                        cell.Next = _cellGroup2;
                        _cellGroup2 = i;
                    }
                    else
                    {
                        cell.Next = _cellGroup4;
                        _cellGroup4 = i;
                    }
                }

                _cellCachesCollection[i] = cell;
            }
            _hasStarCellsH = hasStarCellsU;
            _hasStarCellsV = hasStarCellsV;
            _hasGroup3CellsInAutoRows = hasGroup3CellsInAutoRows;

        }

        /// <summary>
        /// Accumulates length type information for given vertical definition's range.
        /// </summary>
        /// <param name="definitions">Source array of definitions to read values from.</param>
        /// <param name="start">Starting index of the range.</param>
        /// <param name="count">Number of definitions included in the range.</param>
        /// <returns>Length type for given range.</returns>
        private GridUnitType GetVLengthTypeForRange(DefinitionBase[] definitions,
                                                    int start,
                                                    int count)
        {
            Debug.Assert(0 < count && 0 <= start && (start + count) <= definitions.Length);

            GridUnitType lengthType = GridUnitType.None;
            int i = start + count - 1;

            do
            {
                lengthType |= definitions[i].SizeType;
            } while (--i >= start);

            return (lengthType);
        }

        /// <summary>
        /// Accumulates length type information for given vertical definition's range.
        /// </summary>
        /// <param name="definitions">Source array of definitions to read values from.</param>
        /// <param name="start">Starting index of the range.</param>
        /// <param name="count">Number of definitions included in the range.</param>
        /// <returns>Length type for given range.</returns>
        private GridUnitType GetHLengthTypeForRange(DefinitionBase[] definitions,
                                                    int start,
                                                    int count)
        {
            Debug.Assert(0 < count && 0 <= start && (start + count) <= definitions.Length);

            GridUnitType lengthType = GridUnitType.None;
            int i = start + count - 1;

            do
            {
                lengthType |= definitions[i].SizeType;
            } while (--i >= start);

            return (lengthType);
        }

        private double[] CacheMinSizes(int cellsHead, bool isRows)
        {
            double[] minSizes = isRows ? new double[_definitionsV.Length] : new double[_definitionsV.Length];

            for (int j = 0; j < minSizes.Length; j++)
            {
                minSizes[j] = -1;
            }

            int i = cellsHead;
            do
            {
                if (isRows)
                {
                    minSizes[_cellCachesCollection[i].RowIndex] = _definitionsV[_cellCachesCollection[i].RowIndex].RawMinSize;
                }
                else
                {
                    minSizes[_cellCachesCollection[i].ColumnIndex] = _definitionsH[_cellCachesCollection[i].ColumnIndex].RawMinSize;
                }

                i = _cellCachesCollection[i].Next;
            } while (i < _cellCachesCollection.Length);

            return minSizes;
        }

        private void ApplyCachedMinSizes(double[] minSizes, bool isRows)
        {
            for (int i = 0; i < minSizes.Length; i++)
            {
                if (DoubleUtils.GreaterThanOrClose(minSizes[i], 0))
                {
                    if (isRows)
                    {
                        _definitionsV[i].SetMinSize(minSizes[i]);
                    }
                    else
                    {
                        _definitionsH[i].SetMinSize(minSizes[i]);
                    }
                }
            }
        }

        private void MeasureCellsGroup(
            int cellsHead,
            Size referenceSize,
            bool ignoreDesiredSizeH,
            bool forceInfinityV)
        {
            bool unusedHasDesiredSizeUChanged;
            MeasureCellsGroup(cellsHead, referenceSize, ignoreDesiredSizeH, forceInfinityV, out unusedHasDesiredSizeUChanged);
        }

        /// <summary>
        /// Measures one group of cells.
        /// </summary>
        /// <param name="cellsHead">Head index of the cells chain.</param>
        /// <param name="referenceSize">Reference size for spanned cells
        /// calculations.</param>
        /// <param name="ignoreDesiredSizeU">When "true" cells' desired
        /// width is not registered in columns.</param>
        /// <param name="forceInfinityV">Passed through to MeasureCell.
        /// When "true" cells' desired height is not registered in rows.</param>
        private void MeasureCellsGroup(
            int cellsHead,
            Size referenceSize,
            bool ignoreDesiredSizeH,
            bool forceInfinityV,
            out bool hasDesiredSizeHChanged)
        {
            hasDesiredSizeHChanged = false;

            if (cellsHead >= _cellCachesCollection.Length)
            {
                return;
            }

            Hashtable spanStore = null!;
            bool ignoreDesiredSizeV = forceInfinityV;

            int i = cellsHead;
            do
            {
                double oldWidth = Children[i].DesiredSize.Width;

                MeasureCell(i, forceInfinityV);

                hasDesiredSizeHChanged |= !DoubleUtils.AreClose(oldWidth,
                                                                Children[i].DesiredSize.Width);

                if (!ignoreDesiredSizeH)
                {
                    if (_cellCachesCollection[i].ColumnSpan == 1)
                    {
                        _definitionsH[_cellCachesCollection[i].ColumnIndex].
                                    UpdateMinSize(Math.Min(Children[i].DesiredSize.Width,
                                                  _definitionsH[_cellCachesCollection[i].ColumnIndex].UserMaxSize));
                    }
                    else
                    {
                        RegisterSpan(
                            ref spanStore,
                            _cellCachesCollection[i].ColumnIndex,
                            _cellCachesCollection[i].ColumnSpan,
                            true,
                            Children[i].DesiredSize.Width);
                    }
                }

                if (!ignoreDesiredSizeV)
                {
                    if (_cellCachesCollection[i].RowSpan == 1)
                    {
                        _definitionsV[_cellCachesCollection[i].RowIndex].
                            UpdateMinSize(Math.Min(Children[i].DesiredSize.Height,
                                          _definitionsV[_cellCachesCollection[i].RowIndex].UserMaxSize));
                    }
                    else
                    {
                        RegisterSpan(
                            ref spanStore,
                            _cellCachesCollection[i].RowIndex,
                            _cellCachesCollection[i].RowSpan,
                            false,
                            Children[i].DesiredSize.Height);
                    }
                }

                i = _cellCachesCollection[i].Next;
            } while (i < _cellCachesCollection.Length);

            if (spanStore != null)
            {
                foreach (DictionaryEntry e in spanStore)
                {
                    SpanKey key = (SpanKey)e.Key;
                    double requestedSize = (double)e.Value;

                    EnsureMinSizeInDefinitionRange(
                        key.H ? _definitionsH : _definitionsV,
                        key.Start,
                        key.Count,
                        requestedSize,
                        key.H ? referenceSize.Width : referenceSize.Height);
                }
            }
        }

        /// <summary>
        /// Helper method to register a span information for delayed processing.
        /// </summary>
        /// <param name="store">Reference to a hashtable object used as storage.</param>
        /// <param name="start">Span starting index.</param>
        /// <param name="count">Span count.</param>
        /// <param name="u"><c>true</c> if this is a column span. <c>false</c> if this is a row span.</param>
        /// <param name="value">Value to store. If an entry already exists the biggest value is stored.</param>
        private static void RegisterSpan(
            ref Hashtable store,
            int start,
            int count,
            bool u,
            double value)
        {
            if (store == null)
            {
                store = new Hashtable();
            }

            SpanKey key = new SpanKey(start, count, u);
            object o = store[key];

            if (o == null
                || value > (double)o)
            {
                store[key] = value;
            }
        }

        /// <summary>
        /// Takes care of measuring a single cell.
        /// </summary>
        /// <param name="cell">Index of the cell to measure.</param>
        /// <param name="forceInfinityV">If "true" then cell is always
        /// calculated to infinite height.</param>
        private void MeasureCell(
            int cell,
            bool forceInfinityV)
        {
            double cellMeasureWidth;
            double cellMeasureHeight;

            if (_cellCachesCollection[cell].IsAutoH
                && !_cellCachesCollection[cell].IsStarH)
            {
                //  if cell belongs to at least one Auto column and not a single Star column
                //  then it should be calculated "to content", thus it is possible to "shortcut"
                //  calculations and simply assign PositiveInfinity here.
                cellMeasureWidth = double.PositiveInfinity;
            }
            else
            {
                //  otherwise...
                cellMeasureWidth = GetMeasureSizeForRange(
                                        _definitionsH,
                                        _cellCachesCollection[cell].ColumnIndex,
                                        _cellCachesCollection[cell].ColumnSpan);
            }

            if (forceInfinityV)
            {
                cellMeasureHeight = double.PositiveInfinity;
            }
            else if (_cellCachesCollection[cell].IsAutoV
                    && !_cellCachesCollection[cell].IsStarV)
            {
                //  if cell belongs to at least one Auto row and not a single Star row
                //  then it should be calculated "to content", thus it is possible to "shortcut"
                //  calculations and simply assign PositiveInfinity here.
                cellMeasureHeight = double.PositiveInfinity;
            }
            else
            {
                cellMeasureHeight = GetMeasureSizeForRange(
                                        _definitionsV,
                                        _cellCachesCollection[cell].RowIndex,
                                        _cellCachesCollection[cell].RowSpan);
            }

            var child = Children[cell];
            if (child != null)
            {
                Size childConstraint = new Size(cellMeasureWidth, cellMeasureHeight);
                child.Measure(childConstraint);
            }
        }

        /// <summary>
        /// Calculates one dimensional measure size for given definitions' range.
        /// </summary>
        /// <param name="definitions">Source array of definitions to read values from.</param>
        /// <param name="start">Starting index of the range.</param>
        /// <param name="count">Number of definitions included in the range.</param>
        /// <returns>Calculated measure size.</returns>
        /// <remarks>
        /// For "Auto" definitions MinWidth is used in place of PreferredSize.
        /// </remarks>
        private double GetMeasureSizeForRange(
            DefinitionBase[] definitions,
            int start,
            int count)
        {
            Debug.Assert(0 < count && 0 <= start && (start + count) <= definitions.Length);

            double measureSize = 0;
            int i = start + count - 1;

            do
            {
                RowDefinition? rowDefinition = definitions[i] as RowDefinition;
                ColumnDefinition? columnDefinition = definitions[i] as ColumnDefinition;

                GridUnitType sizeType;
                if (rowDefinition == null)
                    sizeType = columnDefinition!.Width.GridUnitType;
                else
                    sizeType = rowDefinition!.Height.GridUnitType;

                measureSize += (sizeType == GridUnitType.Auto)
                    ? definitions[i].MinSize
                    : definitions[i].MeasureSize;
            } while (--i >= start);

            return (measureSize);
        }

        /// <summary>
        /// Distributes min size back to definition array's range.
        /// </summary>
        /// <param name="start">Start of the range.</param>
        /// <param name="count">Number of items in the range.</param>
        /// <param name="requestedSize">Minimum size that should "fit" into the definitions range.</param>
        /// <param name="definitions">Definition array receiving distribution.</param>
        /// <param name="percentReferenceSize">Size used to resolve percentages.</param>
        private void EnsureMinSizeInDefinitionRange(
            DefinitionBase[] definitions,
            int start,
            int count,
            double requestedSize,
            double percentReferenceSize)
        {
            Debug.Assert(1 < count && 0 <= start && (start + count) <= definitions.Length);

            //  avoid processing when asked to distribute "0"
            if (!IsZero(requestedSize))
            {
                DefinitionBase[] tempDefinitions = TempDefinitions; //  temp array used to remember definitions for sorting
                int end = start + count;
                int autoDefinitionsCount = 0;
                double rangeMinSize = 0;
                double rangePreferredSize = 0;
                double rangeMaxSize = 0;
                double maxMaxSize = 0;                              //  maximum of maximum sizes

                //  first accumulate the necessary information:
                //  a) sum up the sizes in the range;
                //  b) count the number of auto definitions in the range;
                //  c) initialize temp array
                //  d) cache the maximum size into SizeCache
                //  e) accumulate max of max sizes
                for (int i = start; i < end; ++i)
                {
                    double minSize = definitions[i].MinSize;
                    double preferredSize = definitions[i].PreferredSize;
                    double maxSize = Math.Max(definitions[i].UserMaxSize, minSize);

                    rangeMinSize += minSize;
                    rangePreferredSize += preferredSize;
                    rangeMaxSize += maxSize;

                    definitions[i].SizeCache = maxSize;

                    //  sanity check: no matter what, but min size must always be the smaller;
                    //  max size must be the biggest; and preferred should be in between
                    Debug.Assert(minSize <= preferredSize
                                && preferredSize <= maxSize
                                && rangeMinSize <= rangePreferredSize
                                && rangePreferredSize <= rangeMaxSize);

                    if (maxMaxSize < maxSize) maxMaxSize = maxSize;
                    if (definitions[i].UserSize.IsAuto) autoDefinitionsCount++;
                    tempDefinitions[i - start] = definitions[i];
                }

                //  avoid processing if the range already big enough
                if (requestedSize > rangeMinSize)
                {
                    if (requestedSize <= rangePreferredSize)
                    {
                        //
                        //  requestedSize fits into preferred size of the range.
                        //  distribute according to the following logic:
                        //  * do not distribute into auto definitions - they should continue to stay "tight";
                        //  * for all non-auto definitions distribute to equi-size min sizes, without exceeding preferred size.
                        //
                        //  in order to achieve that, definitions are sorted in a way that all auto definitions
                        //  are first, then definitions follow ascending order with PreferredSize as the key of sorting.
                        //
                        double sizeToDistribute;
                        int i;

                        Array.Sort(tempDefinitions, 0, count, s_spanPreferredDistributionOrderComparer);
                        for (i = 0, sizeToDistribute = requestedSize; i < autoDefinitionsCount; ++i)
                        {
                            //  sanity check: only auto definitions allowed in this loop
                            Debug.Assert(tempDefinitions[i].UserSize.IsAuto);

                            //  adjust sizeToDistribute value by subtracting auto definition min size
                            sizeToDistribute -= (tempDefinitions[i].MinSize);
                        }

                        for (; i < count; ++i)
                        {
                            //  sanity check: no auto definitions allowed in this loop
                            Debug.Assert(!tempDefinitions[i].UserSize.IsAuto);

                            double newMinSize = Math.Min(sizeToDistribute / (count - i), tempDefinitions[i].PreferredSize);
                            if (newMinSize > tempDefinitions[i].MinSize) { tempDefinitions[i].UpdateMinSize(newMinSize); }
                            sizeToDistribute -= newMinSize;
                        }

                        //  sanity check: requested size must all be distributed
                        Debug.Assert(IsZero(sizeToDistribute));
                    }
                    else if (requestedSize <= rangeMaxSize)
                    {
                        //
                        //  requestedSize bigger than preferred size, but fit into max size of the range.
                        //  distribute according to the following logic:
                        //  * do not distribute into auto definitions, if possible - they should continue to stay "tight";
                        //  * for all non-auto definitions distribute to euqi-size min sizes, without exceeding max size.
                        //
                        //  in order to achieve that, definitions are sorted in a way that all non-auto definitions
                        //  are last, then definitions follow ascending order with MaxSize as the key of sorting.
                        //
                        double sizeToDistribute;
                        int i;

                        Array.Sort(tempDefinitions, 0, count, s_spanMaxDistributionOrderComparer);
                        for (i = 0, sizeToDistribute = requestedSize - rangePreferredSize; i < count - autoDefinitionsCount; ++i)
                        {
                            //  sanity check: no auto definitions allowed in this loop
                            Debug.Assert(!tempDefinitions[i].UserSize.IsAuto);

                            double preferredSize = tempDefinitions[i].PreferredSize;
                            double newMinSize = preferredSize + sizeToDistribute / (count - autoDefinitionsCount - i);
                            tempDefinitions[i].UpdateMinSize(Math.Min(newMinSize, tempDefinitions[i].SizeCache));
                            sizeToDistribute -= (tempDefinitions[i].MinSize - preferredSize);
                        }

                        for (; i < count; ++i)
                        {
                            //  sanity check: only auto definitions allowed in this loop
                            Debug.Assert(tempDefinitions[i].UserSize.IsAuto);

                            double preferredSize = tempDefinitions[i].MinSize;
                            double newMinSize = preferredSize + sizeToDistribute / (count - i);
                            tempDefinitions[i].UpdateMinSize(Math.Min(newMinSize, tempDefinitions[i].SizeCache));
                            sizeToDistribute -= (tempDefinitions[i].MinSize - preferredSize);
                        }

                        //  sanity check: requested size must all be distributed
                        Debug.Assert(IsZero(sizeToDistribute));
                    }
                    else
                    {
                        //
                        //  requestedSize bigger than max size of the range.
                        //  distribute according to the following logic:
                        //  * for all definitions distribute to equi-size min sizes.
                        //
                        double equalSize = requestedSize / count;

                        if (equalSize < maxMaxSize
                            && !AreClose(equalSize, maxMaxSize))
                        {
                            //  equi-size is less than maximum of maxSizes.
                            //  in this case distribute so that smaller definitions grow faster than
                            //  bigger ones.
                            double totalRemainingSize = maxMaxSize * count - rangeMaxSize;
                            double sizeToDistribute = requestedSize - rangeMaxSize;

                            //  sanity check: totalRemainingSize and sizeToDistribute must be real positive numbers
                            Debug.Assert(!double.IsInfinity(totalRemainingSize)
                                        && !DoubleUtils.IsNaN(totalRemainingSize)
                                        && totalRemainingSize > 0
                                        && !double.IsInfinity(sizeToDistribute)
                                        && !DoubleUtils.IsNaN(sizeToDistribute)
                                        && sizeToDistribute > 0);

                            for (int i = 0; i < count; ++i)
                            {
                                double deltaSize = (maxMaxSize - tempDefinitions[i].SizeCache) * sizeToDistribute / totalRemainingSize;
                                tempDefinitions[i].UpdateMinSize(tempDefinitions[i].SizeCache + deltaSize);
                            }
                        }
                        else
                        {
                            //
                            //  equi-size is greater or equal to maximum of max sizes.
                            //  all definitions receive equalSize as their mim sizes.
                            //
                            for (int i = 0; i < count; ++i)
                            {
                                tempDefinitions[i].UpdateMinSize(equalSize);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolves Star's for given array of definitions.
        /// </summary>
        /// <param name="definitions">Array of definitions to resolve stars.</param>
        /// <param name="availableSize">All available size.</param>
        /// <remarks>
        /// Must initialize LayoutSize for all Star entries in given array of definitions.
        /// </remarks>
        private void ResolveStar(
            DefinitionBase[] definitions,
            double availableSize)
        {
            //if (FrameworkAppContextSwitches.GridStarDefinitionsCanExceedAvailableSpace)
            //{
            //    ResolveStarLegacy(definitions, availableSize);
            //}
            //else
            //{
            ResolveStarMaxDiscrepancy(definitions, availableSize);
            //}
        }

        private List<ColumnDefinition> GetColumnDefinitions(string columns)
        {
            if (string.IsNullOrEmpty(columns))
                columns = "*";
            var colStrs = columns.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return colStrs.Select(s => new ColumnDefinition(GridLength.Parse(s))).ToList();
        }

        private List<RowDefinition> GetRowDefinitions(string rows)
        {
            if (string.IsNullOrEmpty(rows))
                rows = "*";
            var rowStrs = rows.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return rowStrs.Select(s => new RowDefinition(GridLength.Parse(s))).ToList();
        }

        // new implementation as of 4.7.  Several improvements:
        // 1. Allocate to *-defs hitting their min or max constraints, before allocating
        //      to other *-defs.  A def that hits its min uses more space than its
        //      proportional share, reducing the space available to everyone else.
        //      The legacy algorithm deducted this space only from defs processed
        //      after the min;  the new algorithm deducts it proportionally from all
        //      defs.   This avoids the "*-defs exceed available space" problem,
        //      and other related problems where *-defs don't receive proportional
        //      allocations even though no constraints are preventing it.
        // 2. When multiple defs hit min or max, resolve the one with maximum
        //      discrepancy (defined below).   This avoids discontinuities - small
        //      change in available space resulting in large change to one def's allocation.
        // 3. Correct handling of large *-values, including Infinity.
        private void ResolveStarMaxDiscrepancy(
            DefinitionBase[] definitions,
            double availableSize)
        {
            int defCount = definitions.Length;
            DefinitionBase[] tempDefinitions = TempDefinitions;
            int minCount = 0, maxCount = 0;
            double takenSize = 0;
            double totalStarWeight = 0.0;
            int starCount = 0;      // number of unresolved *-definitions
            double scale = 1.0;     // scale factor applied to each *-weight;  negative means "Infinity is present"

            // Phase 1.  Determine the maximum *-weight and prepare to adjust *-weights
            double maxStar = 0.0;
            for (int i = 0; i < defCount; ++i)
            {
                DefinitionBase def = definitions[i];

                if (def.SizeType == GridUnitType.Star)
                {
                    ++starCount;
                    def.MeasureSize = 1.0;  // meaning "not yet resolved in phase 3"
                    if (def.UserSize.Value > maxStar)
                    {
                        maxStar = def.UserSize.Value;
                    }
                }
            }

            if (Double.IsPositiveInfinity(maxStar))
            {
                // negative scale means one or more of the weights was Infinity
                scale = -1.0;
            }
            else if (starCount > 0)
            {
                // if maxStar * starCount > Double.Max, summing all the weights could cause
                // floating-point overflow.  To avoid that, scale the weights by a factor to keep
                // the sum within limits.  Choose a power of 2, to preserve precision.
                double power = Math.Floor(Math.Log(Double.MaxValue / maxStar / starCount, 2.0));
                if (power < 0.0)
                {
                    scale = Math.Pow(2.0, power - 4.0); // -4 is just for paranoia
                }
            }

            // normally Phases 2 and 3 execute only once.  But certain unusual combinations of weights
            // and constraints can defeat the algorithm, in which case we repeat Phases 2 and 3.
            // More explanation below...
            for (bool runPhase2and3 = true; runPhase2and3;)
            {
                // Phase 2.   Compute total *-weight W and available space S.
                // For *-items that have Min or Max constraints, compute the ratios used to decide
                // whether proportional space is too big or too small and add the item to the
                // corresponding list.  (The "min" list is in the first half of tempDefinitions,
                // the "max" list in the second half.  TempDefinitions has capacity at least
                // 2*defCount, so there's room for both lists.)
                totalStarWeight = 0.0;
                takenSize = 0.0;
                minCount = maxCount = 0;

                for (int i = 0; i < defCount; ++i)
                {
                    DefinitionBase def = definitions[i];

                    switch (def.SizeType)
                    {
                        case (GridUnitType.Auto):
                            takenSize += definitions[i].MinSize;
                            break;
                        case (GridUnitType.Pixel):
                            takenSize += def.MeasureSize;
                            break;
                        case (GridUnitType.Star):
                            if (def.MeasureSize < 0.0)
                            {
                                takenSize += -def.MeasureSize;  // already resolved
                            }
                            else
                            {
                                double starWeight = StarWeight(def, scale);
                                totalStarWeight += starWeight;

                                if (def.MinSize > 0.0)
                                {
                                    // store ratio w/min in MeasureSize (for now)
                                    tempDefinitions[minCount++] = def;
                                    def.MeasureSize = starWeight / def.MinSize;
                                }

                                double effectiveMaxSize = Math.Max(def.MinSize, def.UserMaxSize);
                                if (!Double.IsPositiveInfinity(effectiveMaxSize))
                                {
                                    // store ratio w/max in SizeCache (for now)
                                    tempDefinitions[defCount + maxCount++] = def;
                                    def.SizeCache = starWeight / effectiveMaxSize;
                                }
                            }
                            break;
                    }
                }

                // Phase 3.  Resolve *-items whose proportional sizes are too big or too small.
                int minCountPhase2 = minCount, maxCountPhase2 = maxCount;
                double takenStarWeight = 0.0;
                double remainingAvailableSize = availableSize - takenSize;
                double remainingStarWeight = totalStarWeight - takenStarWeight;
                Array.Sort(tempDefinitions, 0, minCount, s_minRatioComparer);
                Array.Sort(tempDefinitions, defCount, maxCount, s_maxRatioComparer);

                while (minCount + maxCount > 0 && remainingAvailableSize > 0.0)
                {
                    // the calculation
                    //            remainingStarWeight = totalStarWeight - takenStarWeight
                    // is subject to catastrophic cancellation if the two terms are nearly equal,
                    // which leads to meaningless results.   Check for that, and recompute from
                    // the remaining definitions.   [This leads to quadratic behavior in really
                    // pathological cases - but they'd never arise in practice.]
                    const double starFactor = 1.0 / 256.0;      // lose more than 8 bits of precision -> recalculate
                    if (remainingStarWeight < totalStarWeight * starFactor)
                    {
                        takenStarWeight = 0.0;
                        totalStarWeight = 0.0;

                        for (int i = 0; i < defCount; ++i)
                        {
                            DefinitionBase def = definitions[i];
                            if (def.SizeType == GridUnitType.Star && def.MeasureSize > 0.0)
                            {
                                totalStarWeight += StarWeight(def, scale);
                            }
                        }

                        remainingStarWeight = totalStarWeight - takenStarWeight;
                    }

                    double minRatio = (minCount > 0) ? tempDefinitions[minCount - 1].MeasureSize : Double.PositiveInfinity;
                    double maxRatio = (maxCount > 0) ? tempDefinitions[defCount + maxCount - 1].SizeCache : -1.0;

                    // choose the def with larger ratio to the current proportion ("max discrepancy")
                    double proportion = remainingStarWeight / remainingAvailableSize;
                    bool? chooseMin = Choose(minRatio, maxRatio, proportion);

                    // if no def was chosen, advance to phase 4;  the current proportion doesn't
                    // conflict with any min or max values.
                    if (!(chooseMin.HasValue))
                    {
                        break;
                    }

                    // get the chosen definition and its resolved size
                    DefinitionBase resolvedDef;
                    double resolvedSize;
                    if (chooseMin == true)
                    {
                        resolvedDef = tempDefinitions[minCount - 1];
                        resolvedSize = resolvedDef.MinSize;
                        --minCount;
                    }
                    else
                    {
                        resolvedDef = tempDefinitions[defCount + maxCount - 1];
                        resolvedSize = Math.Max(resolvedDef.MinSize, resolvedDef.UserMaxSize);
                        --maxCount;
                    }

                    // resolve the chosen def, deduct its contributions from W and S.
                    // Defs resolved in phase 3 are marked by storing the negative of their resolved
                    // size in MeasureSize, to distinguish them from a pending def.
                    takenSize += resolvedSize;
                    resolvedDef.MeasureSize = -resolvedSize;
                    takenStarWeight += StarWeight(resolvedDef, scale);
                    --starCount;

                    remainingAvailableSize = availableSize - takenSize;
                    remainingStarWeight = totalStarWeight - takenStarWeight;

                    // advance to the next candidate defs, removing ones that have been resolved.
                    // Both counts are advanced, as a def might appear in both lists.
                    while (minCount > 0 && tempDefinitions[minCount - 1].MeasureSize < 0.0)
                    {
                        --minCount;
                        tempDefinitions[minCount] = null;
                    }
                    while (maxCount > 0 && tempDefinitions[defCount + maxCount - 1].MeasureSize < 0.0)
                    {
                        --maxCount;
                        tempDefinitions[defCount + maxCount] = null;
                    }
                }

                // decide whether to run Phase2 and Phase3 again.  There are 3 cases:
                // 1. There is space available, and *-defs remaining.  This is the
                //      normal case - move on to Phase 4 to allocate the remaining
                //      space proportionally to the remaining *-defs.
                // 2. There is space available, but no *-defs.  This implies at least one
                //      def was resolved as 'max', taking less space than its proportion.
                //      If there are also 'min' defs, reconsider them - we can give
                //      them more space.   If not, all the *-defs are 'max', so there's
                //      no way to use all the available space.
                // 3. We allocated too much space.   This implies at least one def was
                //      resolved as 'min'.  If there are also 'max' defs, reconsider
                //      them, otherwise the over-allocation is an inevitable consequence
                //      of the given min constraints.
                // Note that if we return to Phase2, at least one *-def will have been
                // resolved.  This guarantees we don't run Phase2+3 infinitely often.
                runPhase2and3 = false;
                if (starCount == 0 && takenSize < availableSize)
                {
                    // if no *-defs remain and we haven't allocated all the space, reconsider the defs
                    // resolved as 'min'.   Their allocation can be increased to make up the gap.
                    for (int i = minCount; i < minCountPhase2; ++i)
                    {
                        DefinitionBase def = tempDefinitions[i];
                        if (def != null)
                        {
                            def.MeasureSize = 1.0;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;       // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }

                if (takenSize > availableSize)
                {
                    // if we've allocated too much space, reconsider the defs
                    // resolved as 'max'.   Their allocation can be decreased to make up the gap.
                    for (int i = maxCount; i < maxCountPhase2; ++i)
                    {
                        DefinitionBase def = tempDefinitions[defCount + i];
                        if (def != null)
                        {
                            def.MeasureSize = 1.0;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;    // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }
            }

            // Phase 4.  Resolve the remaining defs proportionally.
            starCount = 0;
            for (int i = 0; i < defCount; ++i)
            {
                DefinitionBase def = definitions[i];

                if (def.SizeType == GridUnitType.Star)
                {
                    if (def.MeasureSize < 0.0)
                    {
                        // this def was resolved in phase 3 - fix up its measure size
                        def.MeasureSize = -def.MeasureSize;
                    }
                    else
                    {
                        // this def needs resolution, add it to the list, sorted by *-weight
                        tempDefinitions[starCount++] = def;
                        def.MeasureSize = StarWeight(def, scale);
                    }
                }
            }

            if (starCount > 0)
            {
                Array.Sort(tempDefinitions, 0, starCount, s_starWeightComparer);

                // compute the partial sums of *-weight, in increasing order of weight
                // for minimal loss of precision.
                totalStarWeight = 0.0;
                for (int i = 0; i < starCount; ++i)
                {
                    DefinitionBase def = tempDefinitions[i];
                    totalStarWeight += def.MeasureSize;
                    def.SizeCache = totalStarWeight;
                }

                // resolve the defs, in decreasing order of weight
                for (int i = starCount - 1; i >= 0; --i)
                {
                    DefinitionBase def = tempDefinitions[i];
                    double resolvedSize = (def.MeasureSize > 0.0) ? Math.Max(availableSize - takenSize, 0.0) * (def.MeasureSize / def.SizeCache) : 0.0;

                    // min and max should have no effect by now, but just in case...
                    resolvedSize = Math.Min(resolvedSize, def.UserMaxSize);
                    resolvedSize = Math.Max(def.MinSize, resolvedSize);

                    def.MeasureSize = resolvedSize;
                    takenSize += resolvedSize;
                }
            }
        }

        /// <summary>
        /// Calculates desired size for given array of definitions.
        /// </summary>
        /// <param name="definitions">Array of definitions to use for calculations.</param>
        /// <returns>Desired size.</returns>
        private double CalculateDesiredSize(
            DefinitionBase[] definitions)
        {
            double desiredSize = 0;

            for (int i = 0; i < definitions.Length; ++i)
            {
                desiredSize += definitions[i].MinSize;
            }

            return (desiredSize);
        }

        /// <summary>
        /// Returns *-weight, adjusted for scale computed during Phase 1
        /// </summary>
        static double StarWeight(DefinitionBase def, double scale)
        {
            if (scale < 0.0)
            {
                // if one of the *-weights is Infinity, adjust the weights by mapping
                // Infinity to 1.0 and everything else to 0.0:  the infinite items share the
                // available space equally, everyone else gets nothing.
                return (Double.IsPositiveInfinity(def.UserSize.Value)) ? 1.0 : 0.0;
            }
            else
            {
                return def.UserSize.Value * scale;
            }
        }

        /// <summary>
        /// Choose the ratio with maximum discrepancy from the current proportion.
        /// Returns:
        ///     true    if proportion fails a min constraint but not a max, or
        ///                 if the min constraint has higher discrepancy
        ///     false   if proportion fails a max constraint but not a min, or
        ///                 if the max constraint has higher discrepancy
        ///     null    if proportion doesn't fail a min or max constraint
        /// The discrepancy is the ratio of the proportion to the max- or min-ratio.
        /// When both ratios hit the constraint,  minRatio < proportion < maxRatio,
        /// and the minRatio has higher discrepancy if
        ///         (proportion / minRatio) > (maxRatio / proportion)
        /// </summary>
        private static bool? Choose(double minRatio, double maxRatio, double proportion)
        {
            if (minRatio < proportion)
            {
                if (maxRatio > proportion)
                {
                    // compare proportion/minRatio : maxRatio/proportion, but
                    // do it carefully to avoid floating-point overflow or underflow
                    // and divide-by-0.
                    double minPower = Math.Floor(Math.Log(minRatio, 2.0));
                    double maxPower = Math.Floor(Math.Log(maxRatio, 2.0));
                    double f = Math.Pow(2.0, Math.Floor((minPower + maxPower) / 2.0));
                    if ((proportion / f) * (proportion / f) > (minRatio / f) * (maxRatio / f))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else if (maxRatio > proportion)
            {
                return false;
            }

            return null;
        }

        /// <summary>
        /// CellCache stored calculated values of
        /// 1. attached cell positioning properties;
        /// 2. size type;
        /// 3. index of a next cell in the group;
        /// </summary>
        private struct CellCache
        {
            internal int ColumnIndex;
            internal int RowIndex;
            internal int ColumnSpan;
            internal int RowSpan;
            internal GridUnitType SizeTypeH;
            internal GridUnitType SizeTypeV;
            internal int Next;
            internal bool IsStarH { get { return ((SizeTypeH & GridUnitType.Star) != 0); } }
            internal bool IsAutoH { get { return ((SizeTypeH & GridUnitType.Auto) != 0); } }
            internal bool IsStarV { get { return ((SizeTypeV & GridUnitType.Star) != 0); } }
            internal bool IsAutoV { get { return ((SizeTypeV & GridUnitType.Auto) != 0); } }
        }

        /// <summary>
        /// Helper class for representing a key for a span in hashtable.
        /// </summary>
        private class SpanKey
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="start">Starting index of the span.</param>
            /// <param name="count">Span count.</param>
            /// <param name="h"><c>true</c> for columns; <c>false</c> for rows.</param>
            internal SpanKey(int start, int count, bool h)
            {
                _start = start;
                _count = count;
                _h = h;
            }

            /// <summary>
            /// <see cref="object.GetHashCode"/>
            /// </summary>
            public override int GetHashCode()
            {
                int hash = (_start ^ (_count << 2));

                if (_h) hash &= 0x7ffffff;
                else hash |= 0x8000000;

                return (hash);
            }

            /// <summary>
            /// <see cref="object.Equals(object)"/>
            /// </summary>
            public override bool Equals(object obj)
            {
                SpanKey sk = obj as SpanKey;
                return (sk != null
                        && sk._start == _start
                        && sk._count == _count
                        && sk._h == _h);
            }

            /// <summary>
            /// Returns start index of the span.
            /// </summary>
            internal int Start { get { return (_start); } }

            /// <summary>
            /// Returns span count.
            /// </summary>
            internal int Count { get { return (_count); } }

            /// <summary>
            /// Returns <c>true</c> if this is a column span.
            /// <c>false</c> if this is a row span.
            /// </summary>
            internal bool H { get { return (_h); } }

            private int _start;
            private int _count;
            private bool _h;
        }
        private static bool IsZero(double d)
        {
            return (Math.Abs(d) < c_epsilon);
        }

        /// <summary>
        /// fp version of <c>d1 == d2</c>
        /// </summary>
        /// <param name="d1">First value to compare</param>
        /// <param name="d2">Second value to compare</param>
        /// <returns><c>true</c> if d1 == d2</returns>
        private static bool AreClose(double d1, double d2)
        {
            return (Math.Abs(d1 - d2) < c_epsilon);
        }

        /// <summary>
        /// SpanPreferredDistributionOrderComparer.
        /// </summary>
        private class SpanPreferredDistributionOrderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DefinitionBase definitionX = x as DefinitionBase;
                DefinitionBase definitionY = y as DefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    if (definitionX.UserSize.IsAuto)
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = definitionX.MinSize.CompareTo(definitionY.MinSize);
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = +1;
                        }
                        else
                        {
                            result = definitionX.PreferredSize.CompareTo(definitionY.PreferredSize);
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// SpanMaxDistributionOrderComparer.
        /// </summary>
        private class SpanMaxDistributionOrderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DefinitionBase definitionX = x as DefinitionBase;
                DefinitionBase definitionY = y as DefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    if (definitionX.UserSize.IsAuto)
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                        }
                        else
                        {
                            result = +1;
                        }
                    }
                    else
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = -1;
                        }
                        else
                        {
                            result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// MinRatioComparer.
        /// Sort by w/min (stored in MeasureSize), descending.
        /// We query the list from the back, i.e. in ascending order of w/min.
        /// </summary>
        private class MinRatioComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DefinitionBase definitionX = x as DefinitionBase;
                DefinitionBase definitionY = y as DefinitionBase;

                int result;

                if (!CompareNullRefs(definitionY, definitionX, out result))
                {
                    result = definitionY.MeasureSize.CompareTo(definitionX.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// MaxRatioComparer.
        /// Sort by w/max (stored in SizeCache), ascending.
        /// We query the list from the back, i.e. in descending order of w/max.
        /// </summary>
        private class MaxRatioComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DefinitionBase definitionX = x as DefinitionBase;
                DefinitionBase definitionY = y as DefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                }

                return result;
            }
        }

        /// <summary>
        /// StarWeightComparer.
        /// Sort by *-weight (stored in MeasureSize), ascending.
        /// </summary>
        private class StarWeightComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DefinitionBase definitionX = x as DefinitionBase;
                DefinitionBase definitionY = y as DefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.MeasureSize.CompareTo(definitionY.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// MinRatioIndexComparer.
        /// </summary>
        private class MinRatioIndexComparer : IComparer
        {
            private readonly DefinitionBase[] definitions;

            internal MinRatioIndexComparer(DefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                DefinitionBase definitionX = null;
                DefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }
                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionY, definitionX, out result))
                {
                    result = definitionY.MeasureSize.CompareTo(definitionX.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// MaxRatioIndexComparer.
        /// </summary>
        private class MaxRatioIndexComparer : IComparer
        {
            private readonly DefinitionBase[] definitions;

            internal MaxRatioIndexComparer(DefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                DefinitionBase definitionX = null;
                DefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }
                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                }

                return result;
            }
        }

        /// <summary>
        /// MaxRatioIndexComparer.
        /// </summary>
        private class StarWeightIndexComparer : IComparer
        {
            private readonly DefinitionBase[] definitions;

            internal StarWeightIndexComparer(DefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                DefinitionBase definitionX = null;
                DefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }
                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.MeasureSize.CompareTo(definitionY.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// Helper for Comparer methods.
        /// </summary>
        /// <returns>
        /// true iff one or both of x and y are null, in which case result holds
        /// the relative sort order.
        /// </returns>
        private static bool CompareNullRefs(object x, object y, out int result)
        {
            result = 2;

            if (x == null)
            {
                if (y == null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                if (y == null)
                {
                    result = 1;
                }
            }

            return (result != 2);
        }

    }
}