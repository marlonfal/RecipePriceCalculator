using Domain.Entities;

namespace Application.Parameters
{
    public interface IParameterService
    {
        Parameter Get(Domain.Enums.Parameters parameter);
        decimal Get_Decimal(Domain.Enums.Parameters parameter);
    }
}
