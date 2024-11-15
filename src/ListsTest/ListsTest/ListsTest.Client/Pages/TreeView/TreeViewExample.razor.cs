using ClearBlazor;
using Data;
using ListsTest;

namespace ClearBlazorTest
{
    public partial class TreeViewExample : IAsyncDisposable
    {
        TreeView<TestTreeRow> _tree = null!;
        private TestTreeRow? _selectedItem = null;
        private List<TestTreeRow> _selectedItems = new();
        private SelectionMode _selectionMode = SelectionMode.None;
        private bool _allowSelectionToggle = false;
        private bool _hoverHighlight = true;

        List<TestTreeRow> _localTreeData = ClientData.LocalTestTreeRows100;

        async Task CollapseAll()
        {
            await _tree.CollapseAll();
        }
        async Task ExpandAll()
        {
            await _tree.ExpandAll();
        }
        // async Task OnGoto10Start()
        // {
        //     await List.GotoIndex(10, Alignment.Start);
        // }

        // async Task OnGoto50Center()
        // {
        //     await List.GotoIndex(50, Alignment.Center);
        // }

        // async Task OnGoto70End()
        // {
        //     await List.GotoIndex(70, Alignment.End);
        // }

        // async Task OnAddNewItem()
        // {
        //     var index = _feeds.Count;
        //     _feeds.Add(new Feed(index % 1000, $"Record #{index}", "This is a message"));
        //     await List.Refresh();
        // }

        // async Task OnGotoEnd()
        // {
        //     await List.GotoEnd();
        // }

        // async Task OnGotoStart()
        // {
        //     await List.GotoStart();
        // }

        // async Task OnAddNewItemGotoEndIfAtEnd()
        // {
        //     var atEnd = await List.AtEnd();

        //     var index = _feeds.Count;
        //     _feeds.Add(new Feed(index % 1000, $"Record #{index}", "This is a message"));
        //     await List.Refresh();
        //     if (atEnd)
        //         await List.GotoEnd();
        // }
        private async Task Up1()
        {
            await _tree.Scroll(1);
        }
        private async Task Up5()
        {
            await _tree.Scroll(5);

        }
        private async Task Up10()
        {
            await _tree.Scroll(10);

        }
        private async Task Down1()
        {
            await _tree.Scroll(-1);

        }
        private async Task Down5()
        {
            await _tree.Scroll(-5);

        }
        private async Task Down10()
        {
            await _tree.Scroll(-10);
        }

        private async Task ClearSelections()
        {
            if (_tree == null)
                return;
            await _tree.RemoveAllSelections();
            StateHasChanged();
        }
        private async Task SelectionModeChanged()
        {
            if (_tree == null)
                return;
            await _tree.RemoveAllSelections();
            StateHasChanged();
        }
        public async ValueTask DisposeAsync()
        {
            await _tree.RemoveAllSelections();
        }

    }
}