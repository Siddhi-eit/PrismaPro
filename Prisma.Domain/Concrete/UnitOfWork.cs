using MDFusionLabHaute.Domain.Abstract;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(
            IAccountRepository accountRepository
            , IUserRepository userRepository
            , IExceptionLogRepository exceptionLogRepository
            , ICanistersRepository canisters
            , ICanisterLookupRepository canisterLookup
            , IDispenseRepository dispense
            , IRefillTrackingRepository refillTracking
            , ISanitisingTrakingRepository sanitisingTraking
            , IMachineRepository machineRepository
            , IFormulaRepository formulaRepository)
        {
            Account = accountRepository;
            Users = userRepository;
            ExceptionLog = exceptionLogRepository;
            Canisters = canisters;
            CanisterLookup = canisterLookup;
            Dispense = dispense;
            RefillTracking = refillTracking;
            SanitisingTraking = sanitisingTraking;
            MachineRepository = machineRepository;
            FormulaRepository = formulaRepository;
        }

        public IAccountRepository Account { get; }
        public IUserRepository Users { get; }
        public IExceptionLogRepository ExceptionLog { get; }
        public ICanistersRepository Canisters { get; }

        public ICanisterLookupRepository CanisterLookup { get; }
        public IDispenseRepository Dispense { get; }
        public IRefillTrackingRepository RefillTracking { get; }
        public ISanitisingTrakingRepository SanitisingTraking { get; }
        public IMachineRepository MachineRepository { get; }
        public IFormulaRepository FormulaRepository { get; }
    }
}
