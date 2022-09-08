namespace SimpleToolkit.SimpleShell.Extensions
{
    internal static class SimpleShellExtensions
    {
        public static IEnumerable<ShellSection> GetShellSections(this BaseShellItem baseShellItem)
        {
            var list = new HashSet<ShellSection>();

            if (baseShellItem is ShellSection shellSection)
            {
                list.Add(shellSection);
            }
            else if (baseShellItem is ShellItem shellItem)
            {
                foreach (var item in shellItem.Items)
                {
                    var shellSections = GetShellSections(item);
                    foreach (var section in shellSections)
                        list.Add(section);
                }
            }

            return list;
        }

        public static IEnumerable<ShellContent> GetShellContents(this BaseShellItem baseShellItem)
        {
            var list = new HashSet<ShellContent>();

            if (baseShellItem is ShellContent shellContent)
            {
                list.Add(shellContent);
            }
            else if (baseShellItem is ShellItem shellItem)
            {
                foreach (var item in shellItem.Items)
                {
                    var shellContents = GetShellContents(item);
                    foreach (var content in shellContents)
                        list.Add(content);
                }
            }
            else if (baseShellItem is ShellSection shellSection)
            {
                foreach (var content in shellSection.Items)
                    list.Add(content);
            }

            return list;
        }
    }
}
