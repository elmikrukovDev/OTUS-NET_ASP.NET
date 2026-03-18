using System;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Abstractions.Services;

public interface IPromoCodeService
{
    Task UpdateAppliedPromocodesAsync(Guid id);
}