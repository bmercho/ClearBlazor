namespace ClearBlazor
{
    public class DefinitionBase
    {
        internal DefinitionBase()
        {
            _columnDefinition = this as ColumnDefinition;
            _rowDefinition = this as RowDefinition;
        }

        ColumnDefinition? _columnDefinition;
        RowDefinition? _rowDefinition;
        double _minSize;
        private GridUnitType _sizeType;      //  layout-time user size type. it may differ from _userSizeValueCache.UnitType when calculating "to-content"
        private double _measureSize;         //  size, calculated to be the input constraint size for Child.Measure
        private double _sizeCache;                      //  cache used for various purposes (sorting, caching, etc) during calculations
        private double _offset;                         //  offset of the DefinitionBase from left / top corner (assuming LTR case)

        /// <summary>
        /// Returns min size.
        /// </summary>
        internal double MinSize
        {
            get
            {
                return (_minSize);
            }
        }

        /// <summary>
        /// Internal accessor to user min size field.
        /// </summary>
        internal double UserMinSize
        {
            get
            {
                if (_columnDefinition != null)
                    return _columnDefinition.MinWidth;
                if (_rowDefinition != null)
                    return _rowDefinition.MinHeight;
                return 0;
            }
        }

        /// <summary>
        /// Internal accessor to user max size field.
        /// </summary>
        internal double UserMaxSize
        {
            get
            {
                if (_columnDefinition != null)
                    return _columnDefinition.MaxWidth;
                if (_rowDefinition != null)
                    return _rowDefinition.MaxHeight;
                return 0;
            }
        }

        /// <summary>
        /// Internal accessor to user size field.
        /// </summary>
        internal GridLength UserSize
        {
            get
            {
                if (_columnDefinition != null)
                    return _columnDefinition.Width;
                if (_rowDefinition != null)
                    return _rowDefinition.Height;
                return new GridLength();
            }
        }

        /// <summary>
        /// Layout-time user size type.
        /// </summary>
        internal GridUnitType SizeType
        {
            get { return (_sizeType); }
            set { _sizeType = value; }
        }

        /// <summary>
        /// Returns min size, always taking into account shared state.
        /// </summary>
        internal double MinSizeForArrange
        {
            get
            {
                double minSize = _minSize;
                return minSize;
            }
        }
        /// <summary>
        /// Offset.
        /// </summary>
        internal double FinalOffset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Returns or sets measure size for the definition.
        /// </summary>
        internal double MeasureSize
        {
            get { return (_measureSize); }
            set { _measureSize = value; }
        }

        /// <summary>
        /// Returns definition's layout time type sensitive preferred size.
        /// </summary>
        /// <remarks>
        /// Returned value is guaranteed to be true preferred size.
        /// </remarks>
        internal double PreferredSize
        {
            get
            {
                double preferredSize = MinSize;
                if (_sizeType != GridUnitType.Auto
                    && preferredSize < _measureSize)
                {
                    preferredSize = _measureSize;
                }
                return (preferredSize);
            }
        }
        /// <summary>
        /// Returns or sets size cache for the definition.
        /// </summary>
        internal double SizeCache
        {
            get { return (_sizeCache); }
            set { _sizeCache = value; }
        }

        /// <summary>
        /// Sets min size.
        /// </summary>
        /// <param name="minSize">New size.</param>
        internal void SetMinSize(double minSize)
        {
            _minSize = minSize;
        }

        /// <summary>
        /// Returns min size, never taking into account shared state.
        /// </summary>
        internal double RawMinSize
        {
            get { return _minSize; }
        }

        /// <summary>
        /// Updates min size.
        /// </summary>
        /// <param name="minSize">New size.</param>
        internal void UpdateMinSize(double minSize)
        {
            _minSize = Math.Max(_minSize, minSize);
        }


    }
}
