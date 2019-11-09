using Xunit;
using Apod;
using Apod.Logic.Net.Dtos;
using System;

namespace ApodTests
{
    public class ApodResponseTests
    {
        [Fact]
        public void ApodResponse_CorrectContent_MultipleContent()
        {
            var apodResponse = new ApodResponse(ApodStatusCode.OK, allContent: _allContentSample);

            var expected = _lastContent;

            var actual = apodResponse.Content;

            Assert.Equal(expected.Title, actual.Title);
        }

        [Fact]
        public void ApodResponse_CorrectContent_SingleContent()
        {
            var allContent = new ApodContent[1] { _lastContent };
            var apodResponse = new ApodResponse(ApodStatusCode.OK, allContent: allContent);

            var expected = _lastContent;

            var actual = apodResponse.Content;

            Assert.Equal(expected.Title, actual.Title);
        }

        [Fact]
        public void ApodResponse_CorrectContent_Null()
        {
            var apodResponse = new ApodResponse(ApodStatusCode.OK, allContent: null);

            var actual = apodResponse.Content;

            Assert.Null(actual);
        }

        private readonly ApodContent _lastContent = new ApodContent()
        {
            Copyright = null,
            Date = new DateTime(2008, 11, 02),
            Explanation = "Imagine a pipe as wide as a state and as long as half the Earth.  Now imagine that this pipe is filled with hot gas moving 50,000 kilometers per hour.  Further imagine that this pipe is not made of metal but a transparent magnetic field.  You are envisioning just one of thousands of young spicules on the active Sun.  Pictured above is perhaps the highest resolution image yet of these enigmatic solar flux tubes.  Spicules dot the above frame of solar active region 10380 that crossed the Sun in 2004 June, but are particularly evident as a carpet of dark tubes on the right.  Time-sequenced images have recently shown that spicules last about five minutes, starting out as tall tubes of rapidly rising gas but eventually fading as the gas peaks and falls back down to the Sun.  These images also indicate that the ultimate cause of spicules is sound-like waves that flow over the Sun's surface but leak into the Sun's atmosphere.   digg_url = 'http://apod.nasa.gov/apod/ap081102.html'; digg_skin = 'compact';",
            ContentUrlHD = "https://apod.nasa.gov/apod/image/0811/spicules_sst_big.jpg",
            MediaType = MediaType.Image,
            ServiceVersion = "v1",
            Title = "Spicules: Jets on the Sun",
            ContentUrl = "https://apod.nasa.gov/apod/image/0811/spicules_sst.jpg"
        };

        private readonly ApodContent[] _allContentSample = new ApodContent[]
        {
            new ApodContent()
            {
                Copyright = "Anothy Ayiomamitis",
                Date = new DateTime(2008, 10, 29),
                Explanation = "As far as ghosts go, Mirach's Ghost isn't really that scary. In fact, Mirach's Ghost is just a faint, fuzzy galaxy, well known to astronomers, that happens to be seen nearly along the line-of-sight to Mirach, a bright star. Centered in this star field, Mirach is also called Beta Andromedae. About 200 light-years distant, Mirach is a red giant star, cooler than the Sun but much larger and so intrinsically much brighter than our parent star. In most telescopic views, glare and diffraction spikes tend to hide things that lie near Mirach and make the faint, fuzzy galaxy look like a ghostly internal reflection of the almost overwhelming starlight. Still, appearing in this sharp image just above and to the right, Mirach's Ghost is cataloged as galaxy NGC 404 and is estimated to be some 10 million light-years away.  Take a survey on Aesthetics and Astronomy.   digg_url = 'http://apod.nasa.gov/apod/ap081029.html'; digg_skin = 'compact';",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/0810/mirachs_ayiomamitis_c800.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "Mirach's Ghost",
                ContentUrl = "https://apod.nasa.gov/apod/image/0810/mirachs_ayiomamitis_c800.jpg"
            },

            new ApodContent()
            {
                Copyright = "Giovanni Benintende",
                Date = new DateTime(2008, 10, 30),
                Explanation = "Spooky shapes seem to haunt this starry expanse, drifting through the night in the royal constellation Cepheus. Of course, the shapes are cosmic dust clouds faintly visible in dimly reflected starlight. Far fro your own neighborhood on planet Earth, they lurk at the edge of the Cepheus Flare molecular cloud complex some 1,200 light-years away. Over 2 light-years across and brighter than the other ghostly apparitions, the nebula known as vdB 141 or Sh2-136 near the center of the field is even seen in infrared light. Also cataloged as Bok globule CB230, the core of that cloud is collapsing andis likely a binary star system in the early stages of formation.  Take a survey on Aesthetics and Astronomy.   digg_url = 'http://apod.nasa.gov/apod/ap081030.html'; digg_skin = 'compact';",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/0810/s136crop_benintende.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "Haunting the Cepheus Flare",
                ContentUrl = "https://apod.nasa.gov/apod/image/0810/s136crop_benintende800.jpg"
            },

            new ApodContent()
            {
                Copyright = "Star Shadows",
                Date = new DateTime(2008, 10, 31),
                Explanation = "By starlight this eerie visage shines in the dark, a crooked profile evoking its popular name, the Witch Head Nebula. In fact, this entrancing telescopic portrait gives the impression the witch has fixed her gaze on Orion's bright supergiant star Rigel. Spanning over 50 light-years, the dusty cosmic cloud strongly reflects nearby Rigel's blue light, giving it the characteristic color of a reflection nebula. Cataloged as IC 2118, the Witch Head Nebula is about 1,000 light-years away. Of course, you might see a witch this scary tonight, but don't panic. Have a safe and Happy Halloween!  Take a survey on Aesthetics and Astronomy.   digg_url = 'http://apod.nasa.gov/apod/ap081031.html'; digg_skin = 'compact';",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/0810/ic2118_ssro.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "A Witch by Starlight",
                ContentUrl = "https://apod.nasa.gov/apod/image/0810/ic2118_ssro800.jpg"
            },

            new ApodContent()
            {
                Copyright = "Paul Mortfield",
                Date = new DateTime(2008, 11, 01),
                Explanation = "Menacing flying forms and garish colors are a mark of the Halloween season. They also stand out in this cosmic close-up of the eastern Veil Nebula.  The Veil Nebula itself is a large supernova remnant, the expanding debris cloud from the death explosion of a massive star. While the Veil is roughly circular in shape covering nearly 3 degrees on the sky in the constellation Cygnus, this portion of the eastern Veil spans only 1/2 degree, about the apparent size of the Moon. That translates to 12 light-years at the Veil's estimated distance of 1,400 light-years from planet Earth. In this composite of image data recorded through narrow band filters, emission from hydrogen atoms in the remnant isshown in red with strong emission from oxygen atoms in greenish hues. In the western part of the Veil lies another seasonal apparition, the Witch's Broom.  Take a survey on Aesthetics and Astronomy. digg_url = 'http://apod.nasa.gov/apod/ap081101.html'; digg_skin = 'compact';",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/0811/Veileast-Mortfield-Cancelli.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "A Spectre in the Eastern Veil",
                ContentUrl = "https://apod.nasa.gov/apod/image/0811/Veileast-Mortfield-Cancelli_c800.jpg"
            },

            new ApodContent()
            {
                Copyright = null,
                Date = new DateTime(2008, 11, 02),
                Explanation = "Imagine a pipe as wide as a state and as long as half the Earth.  Now imagine that this pipe is filled with hot gas moving 50,000 kilometers per hour.  Further imagine that this pipe is not made of metal but a transparent magnetic field.  You are envisioning just one of thousands of young spicules on the active Sun.  Pictured above is perhaps the highest resolution image yet of these enigmatic solar flux tubes.  Spicules dot the above frame of solar active region 10380 that crossed the Sun in 2004 June, but are particularly evident as a carpet of dark tubes on the right.  Time-sequenced images have recently shown that spicules last about five minutes, starting out as tall tubes of rapidly rising gas but eventually fading as the gas peaks and falls back down to the Sun.  These images also indicate that the ultimate cause of spicules is sound-like waves that flow over the Sun's surface but leak into the Sun's atmosphere.   digg_url = 'http://apod.nasa.gov/apod/ap081102.html'; digg_skin = 'compact';",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/0811/spicules_sst_big.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "Spicules: Jets on the Sun",
                ContentUrl = "https://apod.nasa.gov/apod/image/0811/spicules_sst.jpg"
            }
        };
    }
}
