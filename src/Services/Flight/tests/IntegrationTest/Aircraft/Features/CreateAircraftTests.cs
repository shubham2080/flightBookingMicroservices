﻿using System.Threading.Tasks;
using BuildingBlocks.Contracts.EventBus.Messages;
using BuildingBlocks.TestBase;
using Flight.Api;
using Flight.Data;
using FluentAssertions;
using Integration.Test.Fakes;
using Xunit;

namespace Integration.Test.Aircraft.Features;

using global::Flight.Aircrafts.Features.CreatingAircraft.V1;

public class CreateAircraftTests : FlightIntegrationTestBase
{
    public CreateAircraftTests(
        TestFixture<Program, FlightDbContext, FlightReadDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_create_new_aircraft_to_db_and_publish_message_to_broker()
    {
        // Arrange
        var command = new FakeCreateAircraftCommand().Generate();

        // Act
        var response = await Fixture.SendAsync(command);

        // Assert
        response?.Id.Should().Be(command.Id);

        (await Fixture.WaitForPublishing<AircraftCreated>()).Should().Be(true);

        (await Fixture.ShouldProcessedPersistInternalCommand<CreateAircraftMongo>()).Should().Be(true);
    }
}
