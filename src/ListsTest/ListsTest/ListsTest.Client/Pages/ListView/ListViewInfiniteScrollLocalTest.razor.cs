using ClearBlazor;
using Data;

namespace ListsTest
{
    public partial class ListViewInfiniteScrollLocalTest
    {
        private TestListRow? _selectedItem = null;
        private List<TestListRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;
        private ListView<TestListRow> _list = null!;
        private bool _atEnd = false;
        private bool _atStart = true;

        List<TestListRow> _localListData = ClientData.LocalTestListRows5000;

        async Task CheckAtStart()
        {
            if (_list == null)
                return;
            _atStart = await _list.AtStart();
            StateHasChanged();
        }

        private async Task OnGotoStart()
        {
            if (_list == null)
                return;
            await _list.GotoStart();
        }

        private async Task SelectionModeChanged()
        {
            if (_list == null)
                return;
            await _list.RemoveAllSelections();

            StateHasChanged();
        }
        private async Task ClearSelections()
        {
            await _list.RemoveAllSelections();
            StateHasChanged();
        }
    }
}