using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        IUserRepository Users { get; }
        IExceptionLogRepository ExceptionLog { get; }
        ICanistersRepository Canisters { get; }
        ICanisterLookupRepository CanisterLookup { get; }
        IDispenseRepository Dispense { get; }
        IRefillTrackingRepository RefillTracking { get; }
        ISanitisingTrakingRepository SanitisingTraking { get; }
        IMachineRepository MachineRepository { get; }
        IFormulaRepository FormulaRepository { get; }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                     