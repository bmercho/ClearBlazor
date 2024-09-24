namespace ClearBlazor
{
    public class SelectionHelper<TItem>
    {
        TItem? _lastSelection;
        int _lastIndex = 0;

        public bool HandleSingleSelect(TItem? item, 
                                       ref TItem? selection,
                                       bool allowSelectionToggle)
        {
            if (selection != null && selection.Equals(item))
            {
                if (allowSelectionToggle)
                {
                    selection = default;
                    return true;
                }
                return false;
            }
            else
            {
                selection = item;
                return true;
            }
        }

        public bool HandleSimpleMultiSelect(TItem? item, 
                                            List<TItem?> selections)
        {
            if (IsSelected(item, selections))
                selections.Remove(item);
            else
                selections.Add(item);
            return true;
        }
        public async Task<bool> HandleMultiSelect(TItem? item, 
                                                  int itemIndex, 
                                                  List<TItem?> selections,
                                                  IList<TItem>? list,
                                                  bool ctrlDown,
                                                  bool shiftDown)
        {
            bool selected = IsSelected(item, selections);
            if (!ctrlDown && !shiftDown)
            {
                _lastSelection = item;
                _lastIndex = itemIndex;
                if (!selected || selections.Count > 0)
                {
                    selections.Clear();
                    selections.Add(item);
                    return true;
                }
                else
                    return false;
            }
            else if (ctrlDown && !shiftDown)
            {
                if (selected)
                    selections.Remove(item);
                else
                    selections.Add(item);
                _lastSelection = item;
                _lastIndex = itemIndex;
                return true;
            }
            else 
            {
                if (list == null)
                    return false;
                if (!ctrlDown)
                    selections.Clear();

                var range = await list.GetSelections(_lastIndex, itemIndex);
                if (range != null && range.Count > 0)
                {
                    foreach (var item1 in range)
                    {
                        if (!IsSelected(item1.Item1, selections))
                            selections.Add(item1.Item1);
                    }
                    return true;
                }
                return true;
            }
        }

        private bool IsSelected(TItem? item, List<TItem?> selections)
        {
            foreach (TItem? item1 in selections)
                if (item1 != null && item1.Equals(item))
                    return true;
            return false;
        }
    }
}
