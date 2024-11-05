using ClearBlazor;
using Data;
using ListsTest;

namespace ClearBlazorTest
{
    public partial class TreeViewExample
    {
        TreeView<TestTreeRow> List = null!;

        List<TestTreeRow> _localTreeData = ClientData.LocalTestTreeRows100;

        async Task CollapseAll()
        {
            await List.CollapseAll();
        }
        async Task ExpandAll()
        {
            await List.ExpandAll();
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
            await List.Scroll(1);
        }
        private async Task Up5()
        {
            await List.Scroll(5);

        }
        private async Task Up10()
        {
            await List.Scroll(10);

        }
        private async Task Down1()
        {
            await List.Scroll(-1);

        }
        private async Task Down5()
        {
            await List.Scroll(-5);

        }
        private async Task Down10()
        {
            await List.Scroll(-10);
        }
    }
}