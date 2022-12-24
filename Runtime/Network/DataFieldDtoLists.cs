using System.Collections.Generic;

namespace mazing.common.Runtime.Network
{
    public class GameFieldListDtoLite
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public List<GameFieldDtoLite> DataFields { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public PaginationDto Pagination { get; set; }
    }
}