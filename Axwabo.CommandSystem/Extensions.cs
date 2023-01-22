using System.Collections.Generic;
using System.Linq;

namespace Axwabo.CommandSystem {

    public static class Extensions {

        public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs) => string.Join(",", hubs.Select(p => p.nicknameSync.MyNick));

        public static string Pluralize(this string word, int count) => $"{count} {(count == 1 ? word : word + "s")}";

    }

}
