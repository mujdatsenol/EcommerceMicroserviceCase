using MediatR;

namespace EcommerceMicroserviceCase.Shared;


public interface IRequestService<T> : IRequest<ServiceResult<T>>;