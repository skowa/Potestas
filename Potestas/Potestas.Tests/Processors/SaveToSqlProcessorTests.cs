﻿using Potestas.Configuration;
using Potestas.Observations;
using Potestas.SqlPlugin.Processors;
using Xunit;

namespace Potestas.Tests.Processors
{
    public class SaveToSqlProcessorTests : BaseSaveToSqlProcessorTests
    {
        //[Fact]
        public void OnNextTest()
        {
            this.OnNextTestInit();
        }

        public override IEnergyObservationProcessor<FlashObservation> CreateObservationProcessor(IConfiguration configuration)
        {
            return new SaveFlashObservationToSqlProcessor(configuration);
        }
    }
}