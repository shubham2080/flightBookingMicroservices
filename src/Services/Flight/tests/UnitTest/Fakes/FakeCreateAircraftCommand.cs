﻿using AutoBogus;

namespace Unit.Test.Fakes;

using global::Flight.Aircrafts.Features.CreatingAircraft.V1;
using MassTransit;

public class FakeCreateAircraftCommand : AutoFaker<CreateAircraft>
{
    public FakeCreateAircraftCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
    }
}
