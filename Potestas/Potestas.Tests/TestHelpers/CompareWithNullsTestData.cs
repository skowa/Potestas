using System.Collections;
using System.Collections.Generic;
using Moq;

namespace Potestas.Tests.TestHelpers
{
    public class CompareWithNullsTestData : IEnumerable<object[]>
    {
        private readonly Mock<IEnergyObservation> _firstObservationMock = new Mock<IEnergyObservation>();
        private readonly Mock<IEnergyObservation> _secondObservationMock = new Mock<IEnergyObservation>();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {_firstObservationMock.Object, null, 1};
            yield return new object[] {null, _secondObservationMock.Object, -1};
            yield return new object[] {null, null, 0};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}