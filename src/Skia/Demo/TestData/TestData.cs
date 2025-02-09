namespace Data
{
    public static class TestData
    {
        public static List<TestListRow> GetTestListRows(int num)
        {
            List<TestListRow> data = new();
            for (int i = 0; i < num; i++)
                data.Add(TestListRow.GetNewTestListRow(i));
            return data;
        }

        public static List<TestTreeRow> GetTestTreeRows(int num)
        {
            List<TestTreeRow> treeRows = new();
            int count = 0;
            for (int i = 0; i <= 9; i++)
            {
                var item = TestTreeRow.GetNewTestTreeRow($"{i + 1}");
                treeRows.Add(item);
                count++;
                for (int j = 0; j <= 9; j++)
                {
                    var item1 = TestTreeRow.GetNewTestTreeRow($"{i + 1}.{j + 1}");
                    item.Children.Add(item1);
                    count++;
                    if (count >= num)
                        return treeRows;

                    for (int k = 0; k <= 9; k++)
                    {
                        var item2 = TestTreeRow.GetNewTestTreeRow($"{i + 1}.{j + 1}.{k + 1}");
                        item1.Children.Add(item2);
                        count++;
                        if (count >= num)
                            return treeRows;

                        for (int l = 0; l <= 9; l++)
                        {
                            var item3 = TestTreeRow.GetNewTestTreeRow($"{i + 1}.{j + 1}.{k + 1}.{l + 1}");
                            item2.Children.Add(item3);
                            for (int m = 0; m <= 10; m++)
                            {
                                var item4 = TestTreeRow.GetNewTestTreeRow($"{i + 1}.{j + 1}.{k + 1}.{l + 1}.{m + 1}");
                                item3.Children.Add(item4);
                                count++;
                                if (count >= num)
                                    return treeRows;
                            }
                        }
                    }
                }
            }
            return treeRows;
        }

        public static List<TestTreeRowFlat> GetTestTreeRowsFlat(List<TestTreeRow> treeData)
        {
            List<TestTreeRowFlat> data = new();
            int index = 0;
            foreach (var row in treeData)
            {
                AddTreeChildren(ref data, row, ref index);
            }
            return data;
        }

        private static void AddTreeChildren(ref List<TestTreeRowFlat> treeData, TestTreeRow row, ref int index)
        {
            treeData.Add(new TestTreeRowFlat()
            {
                ParentId = row.Parent?.ListItemId,
                Index = index++,
                NodeId = row.NodeId,
                ImageId = row.ImageId,
                IconName = row.IconName,
                FirstName = row.FirstName,
                LastName = row.LastName,
                Product = row.Product,
                Available = row.Available,
                Quantity = row.Quantity,
                UnitPrice = row.UnitPrice,
                Notes = row.Notes
            });
            foreach (var child in row.Children)
                AddTreeChildren(ref treeData, child, ref index);
        }
    }
}
