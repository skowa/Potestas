using System.Collections;
using System.Collections.Generic;
using Moq;

namespace Potestas.Tests.TestHelpers
{
    public class EqualsWithNullsTestData : IEnumerable<object[]>
    {
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { _firstObservationMock.Object, null, false };
            yield return new object[] { null, _secondObservationMock.Object, false };
            yield return new object[] { null, null, true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}