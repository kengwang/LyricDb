using LyricDb.Contracts.Models;
using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Dao;
using Riok.Mapperly.Abstractions;

namespace LyricDb.Web.Models.Mappers;

[Mapper]
public partial class LyricToContractMapper : IMapper<Lyric, LyricInfo>
{
    public partial LyricInfo Map(Lyric from);
}

[Mapper]
public partial class UserToContractMapper : IMapper<User, UserInfo>
{
    public partial UserInfo Map(User from);
}

[Mapper]
public partial class SongToContractMapper : IMapper<Song, SongInfo>
{
    public partial SongInfo Map(Song from);
}