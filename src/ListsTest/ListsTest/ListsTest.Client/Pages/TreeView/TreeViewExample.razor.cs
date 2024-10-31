using ClearBlazor;

namespace ClearBlazorTest
{
    public partial class TreeViewExample
    {
        private class Feed : TreeItem<Feed>
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
        }

        TreeView<Feed> List = null!;
        Random random = new Random();

        List<Feed> _feeds = new List<Feed>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            InitialiseData(100);
        }

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
        private void InitialiseData(int maxNum)
        {
            int count = 0;
            for (int i = 0; i <= 4; i++)
            {
                var item = new Feed { Id = $"{i + 1}", Icon = Icons.Material.TwoTone.Folder, Children = new() };
                _feeds.Add(item);
                count++;
                for (int j = 0; j <= 4; j++)
                {
                    var item1 = new Feed { Id = $"{i + 1}.{j + 1}", Icon = Icons.Material.TwoTone.Folder, Children = new() };
                    item.Children.Add(item1);
                    count++;
                    if (count >= maxNum)
                        return;
                    for (int k = 0; k <= 3; k++)
                    {
                        var item2 = new Feed { Id = $"{i + 1}.{j + 1}.{k + 1}", Icon = Icons.Material.TwoTone.Folder, Children = new() };
                        item1.Children.Add(item2);
                        count++;
                        if (count >= maxNum)
                            return;
                    }
                }
            }
        }
    }
}