using System;
using Xunit;

namespace ApodTests
{
    public class ApodContentTests
    {
        [Theory]
        [InlineData("example")]
        [InlineData(31)]
        [InlineData(float.MaxValue)]
        [InlineData(null)]
        [InlineData(new bool[3]{true, true, false})]
        public void Equals_Object_OtherTypeReturnsFalse(object otherObject)
        {
            var input = _exampleContent;

            var result = input.Equals(otherObject);

            Assert.False(result);
        }

        [Fact]
        public void Equals_Object_SameApodObjectReturnsTrue()
        {
            var inputA = _exampleContent;
            var inputB = (object) _exampleContentCopy;

            var result = inputA.Equals(inputB);

            Assert.True(result);
        }

        [Fact]
        public void Equals_ApodContent_NullReturnsFalse()
        {
            var input = _exampleContent;

            var result = input.Equals(null);

            Assert.False(result);
        }

        [Fact]
        public void Equals_ApodContent_SameContentReturnsTrue()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;

            Assert.True(inputA.Equals(inputB));
            Assert.True(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_SameInstanceReturnsTrue()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContent;

            Assert.True(inputA.Equals(inputB));
            Assert.True(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentCopyrightReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Copyright += "some string";

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentDatesReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Date = inputB.Date.AddDays(2);

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentExplanationReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Explanation += "some string";

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentContentUrlHDReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.ContentUrlHD += "some string";

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentMediaTypeReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.MediaType = MediaType.Video; // The exampleContent has MediaType.Image

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentServiceVersionReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.ServiceVersion += "some string";

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentTitleReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Title += "some string";

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void Equals_ApodContent_DifferentContentUrlReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.ContentUrl += "some string";

            Assert.False(inputA.Equals(inputB));
            Assert.False(inputB.Equals(inputA));
        }

        [Fact]
        public void EqualsOperator_BothNull_ReturnsTrue()
        {
            ApodContent inputA = null;
            ApodContent inputB = null;

            Assert.True(inputA == inputB);
        }

        [Fact]
        public void EqualsOperator_InputANull_ReturnsFalse()
        {
            ApodContent inputA = null;
            ApodContent inputB = _exampleContent;

            Assert.False(inputA == inputB);
        }

        [Fact]
        public void EqualsOperator_InputBNull_ReturnsFalse()
        {
            ApodContent inputA = _exampleContent;
            ApodContent inputB = null;

            Assert.False(inputA == inputB);
        }

        [Fact]
        public void EqualsOperator_SameReference_ReturnsTrue()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContent;

            Assert.True(inputA == inputB);
        }

        [Fact]
        public void NotEqualsOperator_BothNull_ReturnsFalse()
        {
            ApodContent inputA = null;
            ApodContent inputB = null;

            Assert.False(inputA != inputB);
        }

        [Fact]
        public void NotEqualsOperator_InputANull_ReturnsTrue()
        {
            ApodContent inputA = null;
            ApodContent inputB = _exampleContent;

            Assert.True(inputA != inputB);
        }

        [Fact]
        public void NotEqualsOperator_InputBNull_ReturnsTrue()
        {
            ApodContent inputA = _exampleContent;
            ApodContent inputB = null;

            Assert.True(inputA != inputB);
        }

        [Fact]
        public void NotEqualsOperator_SameReference_ReturnsFalse()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContent;

            Assert.False(inputA != inputB);
        }

        [Fact]
        public void GetHashCode_SameInstanceHasSameHash()
        {
            var hashA = _exampleContent.GetHashCode();
            var hashB = _exampleContent.GetHashCode();

            Assert.Equal(hashA, hashB);
        }

        [Fact]
        public void GetHashCode_SameContentHasSameHash()
        {
            var hashA = _exampleContent.GetHashCode();
            var hashB = _exampleContentCopy.GetHashCode();

            Assert.Equal(hashA, hashB);
        }

        [Fact]
        public void GetHashCode_DifferentCopyrights_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Copyright += "some string";

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentDates_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Date = inputB.Date.AddDays(2);

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentExplanation_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Explanation += "some string";

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentContentUrlHD_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.ContentUrlHD += "some string";

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentMediaType_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.MediaType = MediaType.Video; // The exampleContent has MediaType.Image

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentServiceVersion_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.ServiceVersion += "some string";

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentTitle_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.Title += "some string";

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentContentUrl_HasDifferentHashes()
        {
            var inputA = _exampleContent;
            var inputB = _exampleContentCopy;
            inputB.ContentUrl += "some string";

            Assert.NotEqual(inputA.GetHashCode(), inputB.GetHashCode());
        }

        private readonly ApodContent _exampleContent = new ApodContent()
        {
            Copyright = "Star Shadows",
            Date = new DateTime(2008, 10, 31),
            Explanation = "By starlight this eerie visage shines in the dark, a crooked profile evoking its popular name, the Witch Head Nebula. In fact, this entrancing telescopic portrait gives the impression the witch has fixed her gaze on Orion's bright supergiant star Rigel. Spanning over 50 light-years, the dusty cosmic cloud strongly reflects nearby Rigel's blue light, giving it the characteristic color of a reflection nebula. Cataloged as IC 2118, the Witch Head Nebula is about 1,000 light-years away. Of course, you might see a witch this scary tonight, but don't panic. Have a safe and Happy Halloween!  Take a survey on Aesthetics and Astronomy.   digg_url = 'http://apod.nasa.gov/apod/ap081031.html'; digg_skin = 'compact';",
            ContentUrlHD = "https://apod.nasa.gov/apod/image/0810/ic2118_ssro.jpg",
            MediaType = MediaType.Image,
            ServiceVersion = "v1",
            Title = "A Witch by Starlight",
            ContentUrl = "https://apod.nasa.gov/apod/image/0810/ic2118_ssro800.jpg"
        };

        private readonly ApodContent _exampleContentCopy = new ApodContent()
        {
            Copyright = "Star Shadows",
            Date = new DateTime(2008, 10, 31),
            Explanation = "By starlight this eerie visage shines in the dark, a crooked profile evoking its popular name, the Witch Head Nebula. In fact, this entrancing telescopic portrait gives the impression the witch has fixed her gaze on Orion's bright supergiant star Rigel. Spanning over 50 light-years, the dusty cosmic cloud strongly reflects nearby Rigel's blue light, giving it the characteristic color of a reflection nebula. Cataloged as IC 2118, the Witch Head Nebula is about 1,000 light-years away. Of course, you might see a witch this scary tonight, but don't panic. Have a safe and Happy Halloween!  Take a survey on Aesthetics and Astronomy.   digg_url = 'http://apod.nasa.gov/apod/ap081031.html'; digg_skin = 'compact';",
            ContentUrlHD = "https://apod.nasa.gov/apod/image/0810/ic2118_ssro.jpg",
            MediaType = MediaType.Image,
            ServiceVersion = "v1",
            Title = "A Witch by Starlight",
            ContentUrl = "https://apod.nasa.gov/apod/image/0810/ic2118_ssro800.jpg"
        };
    }
}
