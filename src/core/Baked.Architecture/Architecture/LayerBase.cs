﻿namespace Baked.Architecture;

public abstract class LayerBase : ILayer
{
    protected ApplicationContext Context { get; private set; } = default!;

    protected virtual string Id => GetType().Name;

    protected virtual IEnumerable<IPhase> GetStartPhases() =>
        [];

    protected virtual IEnumerable<IPhase> GetGeneratePhases() =>
        [];

    protected virtual PhaseContext GetContext(IPhase phase) =>
        phase.CreateEmptyContext();

    string ILayer.Id => Id;

    IEnumerable<IPhase> ILayer.GetStartPhases() =>
        GetStartPhases();

    IEnumerable<IPhase> ILayer.GetGeneratePhases() =>
        GetGeneratePhases();

    PhaseContext ILayer.GetContext(IPhase phase, ApplicationContext context)
    {
        Context = context;

        return GetContext(phase);
    }
}

public abstract class LayerBase<TPhase> : LayerBase
    where TPhase : IPhase
{
    protected override sealed PhaseContext GetContext(IPhase phase) =>
        phase switch
        {
            TPhase tPhase => GetContext(tPhase),
            _ => base.GetContext(phase)
        };

    protected abstract PhaseContext GetContext(TPhase phase);
}

public abstract class LayerBase<TPhase1, TPhase2> : LayerBase
    where TPhase1 : IPhase
    where TPhase2 : IPhase
{
    protected override sealed PhaseContext GetContext(IPhase phase) =>
        phase switch
        {
            TPhase1 tPhase1 => GetContext(tPhase1),
            TPhase2 tPhase2 => GetContext(tPhase2),
            _ => base.GetContext(phase)
        };

    protected abstract PhaseContext GetContext(TPhase1 phase);
    protected abstract PhaseContext GetContext(TPhase2 phase);
}

public abstract class LayerBase<TPhase1, TPhase2, TPhase3> : LayerBase
    where TPhase1 : IPhase
    where TPhase2 : IPhase
    where TPhase3 : IPhase
{
    protected override sealed PhaseContext GetContext(IPhase phase) =>
        phase switch
        {
            TPhase1 tPhase1 => GetContext(tPhase1),
            TPhase2 tPhase2 => GetContext(tPhase2),
            TPhase3 tPhase3 => GetContext(tPhase3),
            _ => base.GetContext(phase)
        };

    protected abstract PhaseContext GetContext(TPhase1 phase);
    protected abstract PhaseContext GetContext(TPhase2 phase);
    protected abstract PhaseContext GetContext(TPhase3 phase);
}