using Dometrain.EFCore.API.Data.ValueGenerator;
using Xunit;


//---generáltatott---

namespace Dometrain.EFCore.API.Tests.Data.ValueGenerator
{
    public class CreatedDateGeneratorTests
    {
        [Fact]
        public void Next_ShouldReturnUtcNow()
        {
            // Arrange
            var generator = new CreatedDateGenerator();
            var before = DateTime.UtcNow;

            // Act
            var result = generator.Next(null!);

            var after = DateTime.UtcNow;

            // Assert
            Assert.True(result >= before);
            Assert.True(result <= after);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void GeneratesStableValues_ShouldBeFalse()
        {
            // Arrange
            var generator = new CreatedDateGenerator();

            // Act & Assert
            Assert.False(generator.GeneratesStableValues);
        }

        [Fact]
        public void GeneratesTemporaryValues_ShouldBeFalse()
        {
            // Arrange
            var generator = new CreatedDateGenerator();

            // Act & Assert
            Assert.False(generator.GeneratesTemporaryValues);
        }
    }
}