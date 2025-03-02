using DirN.Utils.Nodes.Achieved.Utils;
using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("文件过滤", "#f506", "FilterBy")]
    public class HFileFilter : AggregatorHandler<(FilterBy,bool, string, FileInfo[]), FileInfo[]>
    {
        protected override void ExtraInit()
        {
            if(TryGetInputConfig<bool>(out var bConfig))
            {
                bConfig!.Header = "全字匹配";
                bConfig!.Description = "是否全字匹配文件名";
            }

            if (TryGetInputConfig<string>(out var sConfig))
            {
                sConfig!.Header = "过滤格式";
                sConfig!.Description = "通过保留字输入过滤格式，格式为：\n" +
                    "文件名：支持通配符，如 *.txt\n" +
                    "文件大小：支持单位，如 10KB、10MB、10GB\n" +
                    "文件日期：支持相对时间，如 10d、10h、10m、10s\n" +
                    "文件创建时间：支持相对时间，如 10d、10h、10m、10s\n" +
                    "文件修改时间：支持相对时间，如 10d、10h、10m、10s\n" +
                    "文件访问时间：支持相对时间，如 10d、10h、10m、10s\n" +
                    "文件路径：支持通配符，如 *\\*.txt\n" +
                    "文件扩展名：支持通配符，如 *.txt\n" +
                    "文件内容：支持正则表达式，如 ^abc$";
            }

        }

        protected override FileInfo[] Aggregate((FilterBy,bool,string, FileInfo[]) input)
        {
            var (filterBy, bFullMatch, filterStr, fileInfos) = input;
            if (filterBy == FilterBy.Ext)
            {
                return [.. fileInfos.Where(f => bFullMatch? f.Extension.Equals(filterStr, StringComparison.OrdinalIgnoreCase) : f.Extension.Contains(filterStr, StringComparison.OrdinalIgnoreCase))];
            }
            else if (filterBy == FilterBy.Name)
            {
                return [.. fileInfos.Where(f => bFullMatch? f.Name.Equals(filterStr, StringComparison.OrdinalIgnoreCase) : f.Name.Contains(filterStr, StringComparison.OrdinalIgnoreCase))];
            }
            return [];
        }
    }
}
