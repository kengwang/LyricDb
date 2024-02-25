namespace LyricDb.Web.Interfaces;

public interface IMapper<TFrom, TTo>
{
    public TTo Map(TFrom from); 
}