using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Spitzer.Models;
using Xamarin.Forms;

namespace Spitzer.Services
{
    public class NasaMediaLibraryDataStore : IMediaLibrary<MediaItem>
    {
        NasaMediaLibrary library;

        public NasaMediaLibraryDataStore()
        {
            // https://images-api.nasa.gov/search?q=spitzer space telescope&keywords=spitzer space telescope&page=1
            /*
             {
    "collection": {
        "items": [
            {
                "href": "https://images-assets.nasa.gov/image/PIA11796/collection.json",
                "data": [
                    {
                        "title": "Spitzer Space Telescope View of Galaxy Messier 101",
                        "center": "JPL",
                        "nasa_id": "PIA11796",
                        "description_508": "The galaxy Messier 101 is a swirling spiral of stars, gas, and dust. Messier 101 is nearly twice as wide as our Milky Way galaxy in this image as seen by NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Messier 101",
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/STScI",
                        "date_created": "2009-02-10T13:52:01Z",
                        "media_type": "image",
                        "description": "The galaxy Messier 101 is a swirling spiral of stars, gas, and dust. Messier 101 is nearly twice as wide as our Milky Way galaxy in this image as seen by NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11796/PIA11796~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA22089/collection.json",
                "data": [
                    {
                        "title": "Hubble Space Telescope,Spitzer Space Telescope",
                        "center": "JPL",
                        "nasa_id": "PIA22089",
                        "keywords": [
                            "Hubble Space Telescope",
                            "Spitzer Space Telescope",
                            "Orion Nebula"
                        ],
                        "description_508": "This artist's concept showcases both the visible and infrared visualizations of the Orion Nebula, looking down a 'valley' leading to the star cluster at far end.",
                        "secondary_creator": "NNASA/ESA, F. Summers, G. Bacon, Z. Levay, J. DePasquale, L. Frattare, M. Robberto and M. Gennaro (STScI), and R. Hurt (Caltech/IPAC)",
                        "date_created": "2018-01-11T00:00:00Z",
                        "media_type": "image",
                        "description": "This image showcases both the visible and infrared visualizations of the Orion Nebula. This view from a movie sequence looks down the 'valley' leading to the star cluster at the far end. The left side of the image shows the visible-light visualization, which fades to the infrared-light visualization on the right. These two contrasting models derive from observations by the Hubble and Spitzer space telescopes.  An animation is available at https://photojournal.jpl.nasa.gov/catalog/PIA22089"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA22089/PIA22089~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04497/collection.json",
                "data": [
                    {
                        "title": "Help Stamp Out Boring Space Acronyms",
                        "center": "JPL",
                        "nasa_id": "PIA04497",
                        "description_508": "This image was used in a contest to rename the Space InfraRed Telescope Facility SIRTF, now known as the Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL",
                        "date_created": "2003-05-15T23:40:29Z",
                        "media_type": "image",
                        "description": "This image was used in a contest to rename the Space InfraRed Telescope Facility SIRTF, now known as the Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04497/PIA04497~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17444/collection.json",
                "data": [
                    {
                        "title": "Spitzer Trains Its Eyes on Exoplanets Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA17444",
                        "description_508": "This artist concept shows NASA Spitzer Space Telescope surrounded by examples of exoplanets the telescope has examined in over its ten years in space.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2013-09-24T20:30:27Z",
                        "media_type": "image",
                        "description": "This artist concept shows NASA Spitzer Space Telescope surrounded by examples of exoplanets the telescope has examined in over its ten years in space."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17444/PIA17444~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08007/collection.json",
                "data": [
                    {
                        "title": "Galaxy NGC 4579",
                        "center": "JPL",
                        "nasa_id": "PIA08007",
                        "description_508": "Galaxy NGC 4579 was captured by the Spitzer Infrared Nearby Galaxy Survey, or Sings, Legacy project using the Spitzer Space Telescope infrared array camera. I",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Ariz",
                        "date_created": "2006-02-15T00:51:52Z",
                        "media_type": "image",
                        "description": "Galaxy NGC 4579 was captured by the Spitzer Infrared Nearby Galaxy Survey, or Sings, Legacy project using the Spitzer Space Telescope infrared array camera. I"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08007/PIA08007~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13290/collection.json",
                "data": [
                    {
                        "title": "Buckyballs Jiggle Like Jello  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA13290",
                        "description_508": "This artist image illustrates vibrating buckyballs -- spherical molecules of carbon discovered in space for the first time by NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2010-07-22T18:00:02Z",
                        "media_type": "image",
                        "description": "This artist image illustrates vibrating buckyballs -- spherical molecules of carbon discovered in space for the first time by NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13290/PIA13290~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04265/collection.json",
                "data": [
                    {
                        "title": "Finishing Touches for Space Infrared Telescope Facility SIRTF",
                        "center": "JPL",
                        "nasa_id": "PIA04265",
                        "description_508": "Technicians put final touches on NASA Space Infrared Telescope Facility at Lockheed Martin Aeronautics in Sunnyvale, Calif., which launched on August 25, 2003. The telescope is now known as the Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA",
                        "date_created": "2003-03-19T19:16:36Z",
                        "media_type": "image",
                        "description": "Technicians put final touches on NASA Space Infrared Telescope Facility at Lockheed Martin Aeronautics in Sunnyvale, Calif., which launched on August 25, 2003. The telescope is now known as the Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04265/PIA04265~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA02587/collection.json",
                "data": [
                    {
                        "title": "A Shocking Surprise in Stephan Quintet",
                        "center": "JPL",
                        "nasa_id": "PIA02587",
                        "description_508": "The false-color composite image of the Stephan’s Quintet galaxy cluster is made up of data from NASA Spitzer Space Telescope and a ground-based telescope in Spain.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Max Planck Institute",
                        "date_created": "2006-03-03T16:49:04Z",
                        "media_type": "image",
                        "description": "The false-color composite image of the Stephan’s Quintet galaxy cluster is made up of data from NASA Spitzer Space Telescope and a ground-based telescope in Spain."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA02587/PIA02587~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA03031/collection.json",
                "data": [
                    {
                        "title": "Amazing Andromeda in Red",
                        "center": "JPL",
                        "nasa_id": "PIA03031",
                        "description_508": "NASA Spitzer Space Telescope has captured this stunning infrared view of the famous galaxy Messier 31, also known as Andromeda.",
                        "keywords": [
                            "M31",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Ariz.<br />Figure 3: NOAO/AURA/NSF",
                        "date_created": "2005-10-13T17:20:44Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has captured this stunning infrared view of the famous galaxy Messier 31, also known as Andromeda."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA03031/PIA03031~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04267/collection.json",
                "data": [
                    {
                        "title": "SST and the Milky Way, an Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA04267",
                        "description_508": "NASA Spitzer Space Telescope whizzes in front of a brilliant, infrared view of the Milky Way galaxy plane in this artistic depiction.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL",
                        "date_created": "2003-03-22T22:03:28Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope whizzes in front of a brilliant, infrared view of the Milky Way galaxy plane in this artistic depiction."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04267/PIA04267~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04937/collection.json",
                "data": [
                    {
                        "title": "Multi-Wavelength Views of Messier 81",
                        "center": "JPL",
                        "nasa_id": "PIA04937",
                        "description_508": "The magnificent spiral arms of the nearby galaxy Messier 81 are highlighted in this image from NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech/University of Arizona/Harvard-Smithsonian Center for Astrophysics/NOAO/AURA/NSF",
                        "date_created": "2003-12-18T18:14:58Z",
                        "media_type": "image",
                        "description": "The magnificent spiral arms of the nearby galaxy Messier 81 are highlighted in this image from NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04937/PIA04937~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17256/collection.json",
                "data": [
                    {
                        "title": "The Barred Sculptor Galaxy",
                        "center": "JPL",
                        "nasa_id": "PIA17256",
                        "description_508": "The spectacular swirling arms and central bar of the Sculptor galaxy are revealed in this new view from NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2013-08-23T17:05:18Z",
                        "media_type": "image",
                        "description": "The spectacular swirling arms and central bar of the Sculptor galaxy are revealed in this new view from NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17256/PIA17256~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA20069/collection.json",
                "data": [
                    {
                        "title": "Hot N Hotter Planet Measured by Spitzer",
                        "center": "JPL",
                        "nasa_id": "PIA20069",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "description_508": "The varying brightness of an exoplanet called 55 Cancri e is shown in this plot of infrared data captured by NASA Spitzer Space Telescope.",
                        "secondary_creator": "NASA/JPL-Caltech/University of Cambridge",
                        "date_created": "2016-03-30T18:26:02Z",
                        "media_type": "image",
                        "description": "The varying brightness of an exoplanet called 55 Cancri e is shown in this plot of infrared data captured by NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA20069/PIA20069~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18905/collection.json",
                "data": [
                    {
                        "title": "Horsehead of a Different Color",
                        "center": "JPL",
                        "nasa_id": "PIA18905",
                        "description_508": "The famous Horsehead nebula takes on a ghostly appearance in this image from NASA Spitzer Space Telescope, released on December 18, 2014.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2014-12-19T18:29:43Z",
                        "media_type": "image",
                        "description": "The famous Horsehead nebula takes on a ghostly appearance in this image from NASA Spitzer Space Telescope, released on December 18, 2014."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18905/PIA18905~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13900/collection.json",
                "data": [
                    {
                        "title": "Spitzer Sunflower Galaxy",
                        "center": "JPL",
                        "nasa_id": "PIA13900",
                        "description_508": "This view of the Sunflower galaxy highlights a variety of infrared wavelengths captured by NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2011-03-03T23:08:49Z",
                        "media_type": "image",
                        "description": "This view of the Sunflower galaxy highlights a variety of infrared wavelengths captured by NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13900/PIA13900~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18453/collection.json",
                "data": [
                    {
                        "title": "I Spy a Little Asteroid With My Infrared Eye",
                        "center": "JPL",
                        "nasa_id": "PIA18453",
                        "description_508": "This image of asteroid 2011 MD was taken by NASA Spitzer Space Telescope in Feb. 2014, over a period of 20 hours.",
                        "keywords": [
                            "Asteroid",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Northern Arizona University/SAO",
                        "date_created": "2014-06-19T16:45:28Z",
                        "media_type": "image",
                        "description": "This image of asteroid 2011 MD was taken by NASA Spitzer Space Telescope in Feb. 2014, over a period of 20 hours."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18453/PIA18453~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10711/collection.json",
                "data": [
                    {
                        "title": "Ghostly Ring",
                        "center": "JPL",
                        "nasa_id": "PIA10711",
                        "description_508": "NASA Spitzer Space Telescope imaged the mysterious ring around magnetar SGR 1900+14 in infrared light.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2008-05-28T15:10:00Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope imaged the mysterious ring around magnetar SGR 1900+14 in infrared light."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10711/PIA10711~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA05736/collection.json",
                "data": [
                    {
                        "title": "Star Formation in the DR21 Region B",
                        "center": "JPL",
                        "nasa_id": "PIA05736",
                        "description_508": "This image from NASA Spitzer Space Telescope shows an exceptionally bright source of radio emission called DR21.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2004-04-13T17:58:00Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows an exceptionally bright source of radio emission called DR21."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA05736/PIA05736~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04941/collection.json",
                "data": [
                    {
                        "title": "Spectrum from Faint Galaxy IRAS F00183-7111",
                        "center": "JPL",
                        "nasa_id": "PIA04941",
                        "description_508": "NASA Spitzer Space Telescope has detected the building blocks of life in the distant universe, albeit in a violent milieu. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech",
                        "date_created": "2003-12-18T18:15:25Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has detected the building blocks of life in the distant universe, albeit in a violent milieu. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04941/PIA04941~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09561/collection.json",
                "data": [
                    {
                        "title": "Dwarfs in Coma Cluster",
                        "center": "JPL",
                        "nasa_id": "PIA09561",
                        "description_508": "Two large elliptical galaxies, NGC 4889 and NGC 4874 are shown in this image from NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/GSFC/SDSS",
                        "date_created": "2007-05-29T21:38:18Z",
                        "media_type": "image",
                        "description": "Two large elliptical galaxies, NGC 4889 and NGC 4874 are shown in this image from NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09561/PIA09561~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04938/collection.json",
                "data": [
                    {
                        "title": "Long-Wavelength Infrared Views of Messier 81",
                        "center": "JPL",
                        "nasa_id": "PIA04938",
                        "description_508": "The magnificent and dusty spiral arms of the nearby galaxy Messier 81 are highlighted in these NASA Spitzer Space Telescope images. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech/University of Arizona",
                        "date_created": "2003-12-18T18:15:08Z",
                        "media_type": "image",
                        "description": "The magnificent and dusty spiral arms of the nearby galaxy Messier 81 are highlighted in these NASA Spitzer Space Telescope images. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04938/PIA04938~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA19332/collection.json",
                "data": [
                    {
                        "title": "Infographic: Finding Planets With Microlensing",
                        "center": "JPL",
                        "nasa_id": "PIA19332",
                        "description_508": "This infographic explains how NASA Spitzer Space Telescope can be used in tandem with a telescope on the ground to measure the distances to planets discovered using the microlensing technique.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2015-04-14T15:52:02Z",
                        "media_type": "image",
                        "description": "This infographic explains how NASA Spitzer Space Telescope can be used in tandem with a telescope on the ground to measure the distances to planets discovered using the microlensing technique.  http://photojournal.jpl.nasa.gov/catalog/PIA19332"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA19332/PIA19332~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13929/collection.json",
                "data": [
                    {
                        "title": "Undercover Jet Exposed",
                        "center": "JPL",
                        "nasa_id": "PIA13929",
                        "description_508": "This image layout shows two views of the same baby star from NASA Spitzer Space Telescope. Spitzer view shows that this star has a second, identical jet shooting off in the opposite direction of the first.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2011-04-04T22:30:01Z",
                        "media_type": "image",
                        "description": "This image layout shows two views of the same baby star from NASA Spitzer Space Telescope. Spitzer view shows that this star has a second, identical jet shooting off in the opposite direction of the first."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13929/PIA13929~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11390/collection.json",
                "data": [
                    {
                        "title": "Light from Red-Hot Planet",
                        "center": "JPL",
                        "nasa_id": "PIA11390",
                        "description_508": "This figure charts 30 hours of observations taken by NASA Spitzer Space Telescope of a strongly irradiated exoplanet an planet orbiting a star beyond our own. Spitzer measured changes in the planet heat, or infrared light.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/UCSC",
                        "date_created": "2009-01-28T16:59:01Z",
                        "media_type": "image",
                        "description": "This figure charts 30 hours of observations taken by NASA Spitzer Space Telescope of a strongly irradiated exoplanet an planet orbiting a star beyond our own. Spitzer measured changes in the planet heat, or infrared light."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11390/PIA11390~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA01903/collection.json",
                "data": [
                    {
                        "title": "Lighting up a Dead Star Layers",
                        "center": "JPL",
                        "nasa_id": "PIA01903",
                        "description_508": "This image from NASA Spitzer Space Telescope shows the scattered remains of an exploded star named Cassiopeia A. Spitzer infrared detectors picked through these remains and found that much of the star original layering had been preserved. ",
                        "keywords": [
                            "Cassiopeia A",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2006-10-26T17:57:52Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows the scattered remains of an exploded star named Cassiopeia A. Spitzer infrared detectors picked through these remains and found that much of the star original layering had been preserved. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA01903/PIA01903~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13901/collection.json",
                "data": [
                    {
                        "title": "Sunflower Galaxy Glows with Infrared Light",
                        "center": "JPL",
                        "nasa_id": "PIA13901",
                        "description_508": "This image from NASA Spitzer Space Telescope shows infrared light from the Sunflower galaxy, otherwise known as Messier 63. Spitzer view highlights the galaxy dusty spiral arms.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2011-03-03T23:08:50Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows infrared light from the Sunflower galaxy, otherwise known as Messier 63. Spitzer view highlights the galaxy dusty spiral arms."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13901/PIA13901~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09925/collection.json",
                "data": [
                    {
                        "title": "Coronet: A Star-Formation Neighbor",
                        "center": "JPL",
                        "nasa_id": "PIA09925",
                        "description_508": "This composite image shows the Coronet in X-rays from Chandra and infrared from NASA Spitzer Space Telescope orange, green, and cyan. The Spitzer data show young stars plus diffuse emission from dust.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/CXC/JPL-Caltech/CfA",
                        "date_created": "2007-09-13T15:46:23Z",
                        "media_type": "image",
                        "description": "This composite image shows the Coronet in X-rays from Chandra and infrared from NASA Spitzer Space Telescope orange, green, and cyan. The Spitzer data show young stars plus diffuse emission from dust."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09925/PIA09925~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA23123/collection.json",
                "data": [
                    {
                        "title": "A Field of Galaxies Seen by Spitzer and Hubble",
                        "center": "JPL",
                        "nasa_id": "PIA23123",
                        "description": "This deep-field view of the sky, taken by NASA's Spitzer Space Telescope, is dominated by galaxies - including some very faint, very distant ones - circled in red. The bottom right inset shows one of those distant galaxies, made visible thanks to a long-duration observation by Spitzer. The wide-field view also includes data from NASA's Hubble Space Telescope. The Spitzer observations came from the GREATS survey, short for GOODS Re-ionization Era wide-Area Treasury from Spitzer. GOODS is itself an acronym: Great Observatories Origins Deep Survey.  https://photojournal.jpl.nasa.gov/catalog/PIA23123",
                        "keywords": [
                            "Spitzer Space Telescope",
                            "Hubble Space Telescope"
                        ],
                        "description_508": "This deep-field view of the sky taken by NASA's Hubble and Spitzer space telescopes is dominated by galaxies.",
                        "secondary_creator": "NASA/JPL-Caltech/ESA/Spitzer/P. Oesch/S. De Barros/ I.Labbe",
                        "date_created": "2019-05-08T00:00:00Z",
                        "media_type": "image"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA23123/PIA23123~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13316/collection.json",
                "data": [
                    {
                        "title": "NASA Great Observatories Witness a Galactic Spectacle",
                        "center": "JPL",
                        "nasa_id": "PIA13316",
                        "description_508": "This image of two tangled galaxies has been released by NASA Great Observatories. The Antennae galaxies are shown in this composite image from the Chandra X-ray Observatory, the Hubble Space Telescope, and the Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/CXC/SAO/JPL-Caltech/STScI",
                        "date_created": "2010-08-05T17:15:55Z",
                        "media_type": "image",
                        "description": "This image of two tangled galaxies has been released by NASA Great Observatories. The Antennae galaxies are shown in this composite image from the Chandra X-ray Observatory, the Hubble Space Telescope, and the Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13316/PIA13316~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09200/collection.json",
                "data": [
                    {
                        "title": "Exotic Atmospheres  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA09200",
                        "description_508": "This artist concept, based on spectral observations from NASA Hubble Space Telescope and Spitzer Space Telescope, shows a cloudy Jupiter-like planet that orbits very close to its fiery hot star.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2007-02-21T17:50:04Z",
                        "media_type": "image",
                        "description": "This artist concept, based on spectral observations from NASA Hubble Space Telescope and Spitzer Space Telescope, shows a cloudy Jupiter-like planet that orbits very close to its fiery hot star."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09200/PIA09200~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA22918/collection.json",
                "data": [
                    {
                        "title": "Star Gaia 17pbi Seen by Spitzer",
                        "center": "JPL",
                        "nasa_id": "PIA22918",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "description_508": "The location of Gaia 17bpi, which lies in the Sagitta constellation, is indicated in this image taken by NASA's Spitzer Space Telescope.",
                        "secondary_creator": "NASA/JPL-Caltech/M. Kuhn (Caltech)",
                        "date_created": "2018-12-18T00:00:00Z",
                        "media_type": "image",
                        "description": "The location of Gaia 17bpi, which lies in the Sagitta constellation, is indicated in this image taken by NASA's Spitzer Space Telescope.  https://photojournal.jpl.nasa.gov/catalog/PIA22918"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA22918/PIA22918~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA19331/collection.json",
                "data": [
                    {
                        "title": "Time Delay in Microlensing Event",
                        "center": "JPL",
                        "nasa_id": "PIA19331",
                        "description_508": "This plot shows data obtained from NASA Spitzer Space Telescope, and the Optical Gravitational Lensing Experiment telescope located in Chile, during a microlensing event.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Warsaw University Observatory",
                        "date_created": "2015-04-14T15:52:01Z",
                        "media_type": "image",
                        "description": "This plot shows data obtained from NASA's Spitzer Space Telescope and the Optical Gravitational Lensing Experiment, or OGLE, telescope located in Chile, during a \"microlensing\" event. Microlensing events occur when one star passes another, and the gravity of the foreground star causes the distant star's light to magnify and brighten. This magnification is evident in the plot, as both Spitzer and OGLE register an increase in the star's brightness.  If the foreground star is circled by a planet, the planet's gravity can alter the magnification over a shorter period, seen in the plot in the form of spikes and a dip. The great distance between Spitzer, in space, and OGLE, on the ground, meant that Spitzer saw this particular microlensing event before OGLE. The offset in the timing can be used to measure the distance to the planet.  In this case, the planet, called OGLE-2014-BLG-0124L, was found to be 13,000 light-years away, near the center of our Milky Way galaxy.  The finding was the result of fortuitous timing because Spitzer's overall program to observe microlensing events was only just starting up in the week before the planet's effects were visible from Spitzer's vantage point.  While Spitzer sees infrared light of 3.6 microns in wavelength, OGLE sees visible light of 0.8 microns.  http://photojournal.jpl.nasa.gov/catalog/PIA19331"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA19331/PIA19331~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09109/collection.json",
                "data": [
                    {
                        "title": "Unwrapping the Pillars",
                        "center": "JPL",
                        "nasa_id": "PIA09109",
                        "description_508": "This image composite highlights the pillars of the Eagle nebula, as seen in infrared light by NASA Spitzer Space Telescope bottom and visible light by NASA Hubble Space Telescope top insets.",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/STScI/ Institut dAstrophysique Spatiale",
                        "date_created": "2007-01-09T17:56:03Z",
                        "media_type": "image",
                        "description": "This image composite highlights the pillars of the Eagle nebula, as seen in infrared light by NASA Spitzer Space Telescope bottom and visible light by NASA Hubble Space Telescope top insets."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09109/PIA09109~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13134/collection.json",
                "data": [
                    {
                        "title": "Galactic Metropolis",
                        "center": "JPL",
                        "nasa_id": "PIA13134",
                        "description_508": "NASA Spitzer Space Telescope contributed to the infrared component of the observations of a surprisingly large collections of galaxies red dots in center. Shorter-wavelength infrared and visible data are provided by Japan Subaru telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Subaru",
                        "date_created": "2010-05-11T21:58:03Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope contributed to the infrared component of the observations of a surprisingly large collections of galaxies red dots in center. Shorter-wavelength infrared and visible data are provided by Japan Subaru telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13134/PIA13134~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10932/collection.json",
                "data": [
                    {
                        "title": "Super Starburst Galaxy",
                        "center": "JPL",
                        "nasa_id": "PIA10932",
                        "description_508": "The green and red splotch in this image is the most active star-making galaxy in the very distant universe. Nicknamed Baby Boom, it was spotted 12.3 billion light-years away by a suite of telescopes, including NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Subaru/STScI ",
                        "date_created": "2008-07-10T16:52:19Z",
                        "media_type": "image",
                        "description": "The green and red splotch in this image is the most active star-making galaxy in the very distant universe. Nicknamed Baby Boom, it was spotted 12.3 billion light-years away by a suite of telescopes, including NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10932/PIA10932~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA07395/collection.json",
                "data": [
                    {
                        "title": "Fingerprints in the Light",
                        "center": "JPL",
                        "nasa_id": "PIA07395",
                        "description_508": "This spectrum shows the light from a dusty, distant galaxy located 11 billion light-years away. The galaxy is invisible to optical telescopes, but NASA Spitzer Space Telescope captured the light from it and dozens of other similar galaxies.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Cornell",
                        "date_created": "2005-03-01T17:48:59Z",
                        "media_type": "image",
                        "description": "This spectrum shows the light from a dusty, distant galaxy located 11 billion light-years away. The galaxy is invisible to optical telescopes, but NASA Spitzer Space Telescope captured the light from it and dozens of other similar galaxies."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA07395/PIA07395~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA01936/collection.json",
                "data": [
                    {
                        "title": "Fire and Ice Planet  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA01936",
                        "description_508": "This artist animation shows a blistering world revolving around its nearby un. NASA infrared Spitzer Space Telescope observed a planetary system like this one, as the planet sunlit and dark hemispheres swung alternately into the telescope view.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2006-10-12T17:33:44Z",
                        "media_type": "image",
                        "description": "This artist animation shows a blistering world revolving around its nearby un. NASA infrared Spitzer Space Telescope observed a planetary system like this one, as the planet sunlit and dark hemispheres swung alternately into the telescope view."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA01936/PIA01936~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09071/collection.json",
                "data": [
                    {
                        "title": "Stellar Debris in the Large Magellanic Cloud",
                        "center": "JPL",
                        "nasa_id": "PIA09071",
                        "description_508": "This is a composite image of N49, the brightest supernova remnant in optical light in the Large Magellanic Cloud; the image combines data from the Chandra X-ray Telescope blue and NASA Spitzer Space Telescope red.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/CXC/STScI/JPL-Caltech/UIUC/Univ. of Minn.",
                        "date_created": "2006-12-08T23:56:13Z",
                        "media_type": "image",
                        "description": "This is a composite image of N49, the brightest supernova remnant in optical light in the Large Magellanic Cloud; the image combines data from the Chandra X-ray Telescope blue and NASA Spitzer Space Telescope red."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09071/PIA09071~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13789/collection.json",
                "data": [
                    {
                        "title": "The Hidden Galaxy",
                        "center": "JPL",
                        "nasa_id": "PIA13789",
                        "description_508": "Maffei 2 is the poster child for an infrared galaxy that is almost invisible to optical telescopes. But this infrared image from NASA Spitzer Space Telescope penetrates the dust to reveal the galaxy in all its glory.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/UCLA",
                        "date_created": "2011-01-18T23:15:41Z",
                        "media_type": "image",
                        "description": "Maffei 2 is the poster child for an infrared galaxy that is almost invisible to optical telescopes. But this infrared image from NASA Spitzer Space Telescope penetrates the dust to reveal the galaxy in all its glory."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13789/PIA13789~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04943/collection.json",
                "data": [
                    {
                        "title": "Comet Schwassmann-Wachmann I",
                        "center": "JPL",
                        "nasa_id": "PIA04943",
                        "description_508": "NASA Spitzer Space Telescope has captured an image of an unusual comet that experiences frequent outbursts, which produce abrupt changes in brightness.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech/Ames Research Center/University of Arizona",
                        "date_created": "2003-12-18T18:15:38Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has captured an image of an unusual comet that experiences frequent outbursts, which produce abrupt changes in brightness."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04943/PIA04943~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17257/collection.json",
                "data": [
                    {
                        "title": "The Tortured Clouds of Eta Carinae",
                        "center": "JPL",
                        "nasa_id": "PIA17257",
                        "description_508": "Massive stars can wreak havoc on their surroundings, as can be seen in this new view of the Carina nebula from NASAs Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2013-08-23T17:05:18Z",
                        "media_type": "image",
                        "description": "Massive stars can wreak havoc on their surroundings, as can be seen in this new view of the Carina nebula from NASAs Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17257/PIA17257~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12257/collection.json",
                "data": [
                    {
                        "title": "Saturn Infrared Ring  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA12257",
                        "description_508": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Keck",
                        "date_created": "2009-10-07T17:41:38Z",
                        "media_type": "image",
                        "description": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12257/PIA12257~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12377/collection.json",
                "data": [
                    {
                        "title": "Twin Brown Dwarfs Wrapped in a Blanket",
                        "center": "JPL",
                        "nasa_id": "PIA12377",
                        "description_508": "This image from NASA Spitzer Space Telescope shows two young brown dwarfs, objects that fall somewhere between planets and stars in terms of their temperature and mass.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Calar Alto Obsv./Caltech Sub. Obsv.",
                        "date_created": "2009-11-23T00:00:02Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows two young brown dwarfs, objects that fall somewhere between planets and stars in terms of their temperature and mass."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12377/PIA12377~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13551/collection.json",
                "data": [
                    {
                        "title": "Extragalactic Space Balls Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA13551",
                        "description_508": "An infrared photo of the Small Magellanic Cloud taken by NASA Spitzer Space Telescope is shown in this artist illustration; an example of a planetary nebula, and a magnified depiction of buckyballs.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2010-10-27T18:30:45Z",
                        "media_type": "image",
                        "description": "An infrared photo of the Small Magellanic Cloud taken by NASA Spitzer Space Telescope is shown in this artist illustration; an example of a planetary nebula, and a magnified depiction of buckyballs."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13551/PIA13551~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10019/collection.json",
                "data": [
                    {
                        "title": "Dust in the Quasar Wind Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA10019",
                        "description_508": "Astronomers using NASA Spitzer Space Telescope found evidence that such quasar winds might have forged these dusty particles in the very early universe. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2007-10-09T17:26:24Z",
                        "media_type": "image",
                        "description": "Astronomers using NASA Spitzer Space Telescope found evidence that such quasar winds might have forged these dusty particles in the very early universe. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10019/PIA10019~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11173/collection.json",
                "data": [
                    {
                        "title": "Laser-Sharp Jet Splits Water",
                        "center": "JPL",
                        "nasa_id": "PIA11173",
                        "description_508": "A jet of gas firing out of a very young star can be seen ramming into a wall of material in this infrared image from NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Perseus",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Harvard-Smithsonian CfA",
                        "date_created": "2008-09-18T20:41:24Z",
                        "media_type": "image",
                        "description": "A jet of gas firing out of a very young star can be seen ramming into a wall of material in this infrared image from NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11173/PIA11173~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11445/collection.json",
                "data": [
                    {
                        "title": "Celestial Sea of Stars",
                        "center": "JPL",
                        "nasa_id": "PIA11445",
                        "description_508": "NASA Spitzer Space Telescope has captured a new, infrared view of the choppy star-making cloud called M17, or the Swan nebula.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Wisc.",
                        "date_created": "2008-12-08T17:39:44Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has captured a new, infrared view of the choppy star-making cloud called M17, or the Swan nebula. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11445/PIA11445~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18909/collection.json",
                "data": [
                    {
                        "title": "Evolution of a Massive Star",
                        "center": "JPL",
                        "nasa_id": "PIA18909",
                        "description_508": "This series of images show three evolutionary phases of massive star formation, as pictured in infrared images from NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2015-01-27T18:14:13Z",
                        "media_type": "image",
                        "description": "This series of images show three evolutionary phases of massive star formation, as pictured in infrared images from NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18909/PIA18909~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13149/collection.json",
                "data": [
                    {
                        "title": "Blobs House Twin Stars",
                        "center": "JPL",
                        "nasa_id": "PIA13149",
                        "description_508": "This image is one of six images taken by NASA Spitzer Space Telescope, showing that tight-knit twin, or binary stars might be triggered to form by asymmetrical envelopes.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Michigan",
                        "date_created": "2010-05-20T20:55:09Z",
                        "media_type": "image",
                        "description": "This image is one of six images taken by NASA Spitzer Space Telescope, showing that tight-knit twin, or binary stars might be triggered to form by asymmetrical envelopes."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13149/PIA13149~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA14100/collection.json",
                "data": [
                    {
                        "title": "Cosmic Fountain of Crystal Rain",
                        "center": "JPL",
                        "nasa_id": "PIA14100",
                        "description_508": "Using NASA Spitzer Space Telescope, astronomers have, for the first time, found signatures of silicate crystals around a newly forming protostar in the constellation of Orion. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/University of Toledo",
                        "date_created": "2011-05-26T15:55:07Z",
                        "media_type": "image",
                        "description": "Using NASA Spitzer Space Telescope, astronomers have, for the first time, found signatures of silicate crystals around a newly forming protostar in the constellation of Orion. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA14100/PIA14100~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09338/collection.json",
                "data": [
                    {
                        "title": "Spitzer Digs Up Hidden Stars",
                        "center": "JPL",
                        "nasa_id": "PIA09338",
                        "description_508": "Two rambunctious young stars are destroying their natal dust cloud with powerful jets of radiation, in an infrared image from NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Harvard-Smithsonian CfA",
                        "date_created": "2007-05-01T19:35:20Z",
                        "media_type": "image",
                        "description": "Two rambunctious young stars are destroying their natal dust cloud with powerful jets of radiation, in an infrared image from NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09338/PIA09338~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10955/collection.json",
                "data": [
                    {
                        "title": "Peony Nebula Star Settles for Silver Medal",
                        "center": "JPL",
                        "nasa_id": "PIA10955",
                        "description_508": "This image from NASA Spitzer Space Telescope shows he Peony nebula star, a blazing ball of gas shines with the equivalent light of 3.2 million suns.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Potsdam Univ.",
                        "date_created": "2008-07-15T16:55:28Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows he Peony nebula star, a blazing ball of gas shines with the equivalent light of 3.2 million suns."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10955/PIA10955~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA03605/collection.json",
                "data": [
                    {
                        "title": "Dwarf Galaxies Swimming in Tidal Tails",
                        "center": "JPL",
                        "nasa_id": "PIA03605",
                        "description_508": "This false-color infrared image from NASA Spitzer Space Telescope shows little dwarf galaxies forming in the tails of two larger galaxies that are colliding together.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Cornell Univ.",
                        "date_created": "2005-12-01T20:16:12Z",
                        "media_type": "image",
                        "description": "This false-color infrared image from NASA Spitzer Space Telescope shows little dwarf galaxies forming in the tails of two larger galaxies that are colliding together."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA03605/PIA03605~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04942/collection.json",
                "data": [
                    {
                        "title": "Circumstellar Disk Around Fomalhaut",
                        "center": "JPL",
                        "nasa_id": "PIA04942",
                        "description_508": "NASA Spitzer Space Telescope has obtained the first infrared images of the dust disc surrounding Fomalhaut, the 18th brightest star in the sky.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech/Maxwell Telescope",
                        "date_created": "2003-12-18T18:15:35Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has obtained the first infrared images of the dust disc surrounding Fomalhaut, the 18th brightest star in the sky."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04942/PIA04942~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15624/collection.json",
                "data": [
                    {
                        "title": "Measuring Brightness of Super Earth 55 Cancri e",
                        "center": "JPL",
                        "nasa_id": "PIA15624",
                        "description_508": "This graphic illuminates the process by which astronomers using NASA Spitzer Space Telescope have, for the first time, detected the light from a super Earth planet.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2012-05-08T18:30:04Z",
                        "media_type": "image",
                        "description": "This graphic illuminates the process by which astronomers using NASA Spitzer Space Telescope have, for the first time, detected the light from a super Earth planet."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15624/PIA15624~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15808/collection.json",
                "data": [
                    {
                        "title": "Exoplanet is Extremely Hot and Incredibly Close Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA15808",
                        "description_508": "Astronomers using NASA Spitzer Space Telescope have detected what they believe is an alien world just two-thirds the size of Earth -- one of the smallest on record.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2012-07-18T17:00:05Z",
                        "media_type": "image",
                        "description": "Astronomers using NASA Spitzer Space Telescope have detected what they believe is an alien world just two-thirds the size of Earth -- one of the smallest on record."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15808/PIA15808~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11228/collection.json",
                "data": [
                    {
                        "title": "Anatomy of a Busted Comet",
                        "center": "JPL",
                        "nasa_id": "PIA11228",
                        "description_508": "NASA Spitzer Space Telescope captured this picture of comet Holmes in March 2008, five months after the comet suddenly erupted and brightened a millionfold overnight.",
                        "keywords": [
                            "Holmes",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2008-10-13T17:08:42Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope captured this picture of comet Holmes in March 2008, five months after the comet suddenly erupted and brightened a millionfold overnight."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11228/PIA11228~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17259/collection.json",
                "data": [
                    {
                        "title": "Brown Dwarf Backyardigans",
                        "center": "JPL",
                        "nasa_id": "PIA17259",
                        "description_508": "The locations of brown dwarfs discovered by NASA Wide-field Infrared Survey Explorer, or WISE, and mapped by NASA Spitzer Space Telescope, are shown in this diagram as red circles.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2013-09-05T19:43:37Z",
                        "media_type": "image",
                        "description": "The locations of brown dwarfs discovered by NASA Wide-field Infrared Survey Explorer, or WISE, and mapped by NASA Spitzer Space Telescope, are shown in this diagram as red circles."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17259/PIA17259~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18908/collection.json",
                "data": [
                    {
                        "title": "Finding Yellowballs in our Milky Way",
                        "center": "JPL",
                        "nasa_id": "PIA18908",
                        "description_508": "Yellow balls -- which are several hundred to thousands times the size of our solar system -- are pictured here in the center of this image taken by NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2015-01-27T18:14:11Z",
                        "media_type": "image",
                        "description": "Yellow balls -- which are several hundred to thousands times the size of our solar system -- are pictured here in the center of this image taken by NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18908/PIA18908~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04939/collection.json",
                "data": [
                    {
                        "title": "Embedded Outflows from Herbig-Haro 46/47",
                        "center": "JPL",
                        "nasa_id": "PIA04939",
                        "description_508": "This image from NASA Spitzer Space Telescope transforms a dark cloud into a silky translucent veil, revealing the molecular outflow from an otherwise hidden newborn star. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech",
                        "date_created": "2003-12-18T18:15:12Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope transforms a dark cloud into a silky translucent veil, revealing the molecular outflow from an otherwise hidden newborn star. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04939/PIA04939~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA20066/collection.json",
                "data": [
                    {
                        "title": "Simulated Atmosphere of a Hot Gas Giant",
                        "center": "JPL",
                        "nasa_id": "PIA20066",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "description_508": "The turbulent atmosphere of a hot, gaseous planet known as HD 80606b is shown in this simulation based on data from NASA Spitzer Space Telescope.",
                        "secondary_creator": "NASA/JPL-Caltech/MIT/Principia College",
                        "date_created": "2016-03-28T15:22:50Z",
                        "media_type": "image",
                        "description": "The turbulent atmosphere of a hot, gaseous planet known as HD 80606b is shown in this simulation based on data from NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA20066/PIA20066~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17443/collection.json",
                "data": [
                    {
                        "title": "Spitzer Spies a Comet Coma and Tail",
                        "center": "JPL",
                        "nasa_id": "PIA17443",
                        "description_508": "With the help of NASA Spitzer Space Telescope, astronomers have discovered that what was thought to be a large asteroid called Don Quixote is in fact a comet.",
                        "keywords": [
                            "Comet",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/DLR/NAU",
                        "date_created": "2013-09-10T20:13:49Z",
                        "media_type": "image",
                        "description": "With the help of NASA Spitzer Space Telescope, astronomers have discovered that what was thought to be a large asteroid called Don Quixote is in fact a comet."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17443/PIA17443~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18454/collection.json",
                "data": [
                    {
                        "title": "How to Measure the Size of an Asteroid",
                        "center": "JPL",
                        "nasa_id": "PIA18454",
                        "description_508": "Observations of infrared light from NASA Spitzer Space Telescope coming from asteroids provide a better estimate of their true sizes than visible-light measurements.",
                        "keywords": [
                            "Asteroid",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2014-06-19T21:01:06Z",
                        "media_type": "image",
                        "description": "Observations of infrared light from NASA Spitzer Space Telescope coming from asteroids provide a better estimate of their true sizes than visible-light measurements."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18454/PIA18454~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17843/collection.json",
                "data": [
                    {
                        "title": "Speedster Star Shocks the Galaxy",
                        "center": "JPL",
                        "nasa_id": "PIA17843",
                        "description_508": "The red arc in this infrared image from NASA Spitzer Space Telescope is a giant shock wave, created by a speeding star known as Kappa Cassiopeiae.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2014-02-20T23:35:00Z",
                        "media_type": "image",
                        "description": "The red arc in this infrared image from NASA Spitzer Space Telescope is a giant shock wave, created by a speeding star known as Kappa Cassiopeiae."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17843/PIA17843~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09108/collection.json",
                "data": [
                    {
                        "title": "Eagle Nebula Flaunts its Infrared Feathers",
                        "center": "JPL",
                        "nasa_id": "PIA09108",
                        "description_508": "This set of images from NASA Spitzer Space Telescope shows the Eagle nebula in different hues of infrared light. Each view tells a different tale. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/ Institut dAstrophysique Spatiale",
                        "date_created": "2007-01-09T17:56:02Z",
                        "media_type": "image",
                        "description": "This set of images from NASA Spitzer Space Telescope shows the Eagle nebula in different hues of infrared light. Each view tells a different tale. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09108/PIA09108~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12256/collection.json",
                "data": [
                    {
                        "title": "The King of Rings  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA12256",
                        "description_508": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2009-10-07T17:41:37Z",
                        "media_type": "image",
                        "description": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12256/PIA12256~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09070/collection.json",
                "data": [
                    {
                        "title": "Red Giant Plunging Through Space",
                        "center": "JPL",
                        "nasa_id": "PIA09070",
                        "description_508": "This image from NASA Spitzer Space Telescope left panel shows the bow shock of a dying star named R Hydrae, or R Hya, in the constellation Hydra.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/CXC/STScI/JPL-Caltech/UIUC/Univ. of Minn.",
                        "date_created": "2006-12-08T23:45:46Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope left panel shows the bow shock of a dying star named R Hydrae, or R Hya, in the constellation Hydra."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09070/PIA09070~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04934/collection.json",
                "data": [
                    {
                        "title": "Dark Globule in IC 1396 IRAC",
                        "center": "JPL",
                        "nasa_id": "PIA04934",
                        "description_508": "NASA Spitzer Space Telescope image of a glowing stellar nursery provides a spectacular contrast to the opaque cloud seen in visible light inset. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech",
                        "date_created": "2003-12-18T18:14:46Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope image of a glowing stellar nursery provides a spectacular contrast to the opaque cloud seen in visible light inset. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04934/PIA04934~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04724/collection.json",
                "data": [
                    {
                        "title": "SIRTF Aliveness",
                        "center": "JPL",
                        "nasa_id": "PIA04724",
                        "description_508": "This engineering image derives from 100 seconds of observing time on one of the three science instruments aboard the Space InfraRed Telescope Facility SIRTF now known as the Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL/Caltech",
                        "date_created": "2003-09-03T19:22:24Z",
                        "media_type": "image",
                        "description": "This engineering image derives from 100 seconds of observing time on one of the three science instruments aboard the Space InfraRed Telescope Facility (SIRTF). SIRTF was launched on August 25, and opened up its focal plane to starlight on August 30. This image was obtained as part of the instrument power-on sequence on September 1, one week after launch and a full month before the telescope is expected to reach optimal operating temperature and focus. The stars and galaxies seen in this image already attest to the observatory's great sensitivity in the infrared and to its proper operation.  The Space Infrared Telescope Facility (SIRTF) telescope was renamed the Spitzer Space Telescope on December 18, 2003, after the late Dr. Lyman Spitzer, in a contest which was open to the general public.   http://photojournal.jpl.nasa.gov/catalog/PIA04724"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04724/PIA04724~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15423/collection.json",
                "data": [
                    {
                        "title": "Spitzer, Planck and Kepler Extended by NASA Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA15423",
                        "description_508": "From left to right, artist concepts of the Spitzer, Planck and Kepler space telescopes. NASA extended Spitzer and Kepler for two additional years; and the U.S. portion of Planck, a European Space Agency mission, for one year.",
                        "keywords": [
                            "Kepler,Planck,Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2012-04-05T20:50:44Z",
                        "media_type": "image",
                        "description": "From left to right, artist concepts of the Spitzer, Planck and Kepler space telescopes. NASA extended Spitzer and Kepler for two additional years; and the U.S. portion of Planck, a European Space Agency mission, for one year."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15423/PIA15423~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA16690/collection.json",
                "data": [
                    {
                        "title": "Protostar LRLL 54361",
                        "center": "JPL",
                        "nasa_id": "PIA16690",
                        "description_508": "NASA Spitzer and Hubble space telescopes have teamed up to uncover a mysterious infant star that behaves like a police strobe light.",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/ESA/JPL-Caltech/STScI/NOAO/University of Arizona/ Max Planck Institute for Astronomy/University of Massachusetts, Amherst",
                        "date_created": "2013-02-07T18:14:17Z",
                        "media_type": "image",
                        "description": "NASA Spitzer and Hubble space telescopes have teamed up to uncover a mysterious infant star that behaves like a police strobe light."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA16690/PIA16690~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA16612/collection.json",
                "data": [
                    {
                        "title": "Probing Brown Dwarf Layers",
                        "center": "JPL",
                        "nasa_id": "PIA16612",
                        "description_508": "This graph shows the brightness variations of the brown dwarf named 2MASSJ22282889-431026 measured simultaneously by both NASA Hubble and Spitzer space telescopes.",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/University of Arizona",
                        "date_created": "2013-01-08T22:28:12Z",
                        "media_type": "image",
                        "description": "This graph shows the brightness variations of the brown dwarf named 2MASSJ22282889-431026 measured simultaneously by both NASA Hubble and Spitzer space telescopes."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA16612/PIA16612~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17847/collection.json",
                "data": [
                    {
                        "title": "Seeing Through a Veil of Dust",
                        "center": "JPL",
                        "nasa_id": "PIA17847",
                        "description_508": "The closest supernova of its kind to be observed in the last few decades, M82 or the Cigar galaxy, has sparked a global observing campaign involving legions of instruments on the ground and in space, including NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Carnegie Institution for Science",
                        "date_created": "2014-02-26T15:47:14Z",
                        "media_type": "image",
                        "description": "The closest supernova of its kind to be observed in the last few decades, M82 or the Cigar galaxy, has sparked a global observing campaign involving legions of instruments on the ground and in space, including NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17847/PIA17847~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18455/collection.json",
                "data": [
                    {
                        "title": "Solid as a Rock? Porosity of Asteroids",
                        "center": "JPL",
                        "nasa_id": "PIA18455",
                        "description_508": "Observations from NASA Spitzer Space Telescope, taken in infrared light, have helped to reveal that a small asteroid called 2011 MD is made-up of two-thirds empty space.",
                        "keywords": [
                            "Asteroid",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2014-06-19T21:02:06Z",
                        "media_type": "image",
                        "description": "Observations from NASA Spitzer Space Telescope, taken in infrared light, have helped to reveal that a small asteroid called 2011 MD is made-up of two-thirds empty space."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18455/PIA18455~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13289/collection.json",
                "data": [
                    {
                        "title": "Mini Soccer Balls in Space",
                        "center": "JPL",
                        "nasa_id": "PIA13289",
                        "description_508": "This image illustrates that buckyballs -- discovered in space by NASA Spitzer Space Telescope -- closely resemble old fashioned, black-and-white soccer balls, only on much smaller scales. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2010-07-22T18:00:01Z",
                        "media_type": "image",
                        "description": "This image illustrates that buckyballs -- discovered in space by NASA Spitzer Space Telescope -- closely resemble old fashioned, black-and-white soccer balls, only on much smaller scales. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13289/PIA13289~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13287/collection.json",
                "data": [
                    {
                        "title": "Jiggling Soccer-Ball Molecules in Space",
                        "center": "JPL",
                        "nasa_id": "PIA13287",
                        "description_508": "These data from NASA Spitzer Space Telescope show the signatures of buckyballs in space. Buckyballs, also called C60 or buckministerfullerenes, after architect Buckminister Fuller geodesic domes.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/University of Western Ontario",
                        "date_created": "2010-07-22T17:59:58Z",
                        "media_type": "image",
                        "description": "These data from NASA Spitzer Space Telescope show the signatures of buckyballs in space. Buckyballs, also called C60 or buckministerfullerenes, after architect Buckminister Fuller geodesic domes."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13287/PIA13287~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09100/collection.json",
                "data": [
                    {
                        "title": "The Universe First Fireworks",
                        "center": "JPL",
                        "nasa_id": "PIA09100",
                        "description_508": "This is an image from NASA Spitzer Space Telescope of stars and galaxies in the Ursa Major constellation. This infrared image covers a region of space so large that light would take up to 100 million years to travel across it. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/GSFC",
                        "date_created": "2006-12-18T17:55:09Z",
                        "media_type": "image",
                        "description": "This is an image from NASA Spitzer Space Telescope of stars and galaxies in the Ursa Major constellation. This infrared image covers a region of space so large that light would take up to 100 million years to travel across it. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09100/PIA09100~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11392/collection.json",
                "data": [
                    {
                        "title": "Tour of Planet with Extreme Temperature Swings",
                        "center": "JPL",
                        "nasa_id": "PIA11392",
                        "description_508": "This image from NASA Spitzer Space Telescope shows a computer simulation of the planet HD 80606b from an observer located at a point in space lying between the Earth and the HD 80606 system. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/UCSC",
                        "date_created": "2009-01-28T16:59:03Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows a computer simulation of the planet HD 80606b from an observer located at a point in space lying between the Earth and the HD 80606 system. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11392/PIA11392~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15266/collection.json",
                "data": [
                    {
                        "title": "Building a Buckyball Particle in Space Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA15266",
                        "description_508": "NASA Spitzer Space Telescope has detected the solid form of buckyballs in space for the first time. To form a solid particle, the buckyballs must stack together, as illustrated in this artist concept showing the very beginnings of the process.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2012-02-22T15:58:07Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has detected the solid form of buckyballs in space for the first time. To form a solid particle, the buckyballs must stack together, as illustrated in this artist concept showing the very beginnings of the process."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15266/PIA15266~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA03244/collection.json",
                "data": [
                    {
                        "title": "Stellar Snowflake Cluster",
                        "center": "JPL",
                        "nasa_id": "PIA03244",
                        "description_508": "Newborn stars, hidden behind thick dust, are revealed in this image of a section of the Christmas Tree cluster from NASA Spitzer Space Telescope, created in joint effort between Spitzer infrared array camera and multiband imaging photometer instrument",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/CfA",
                        "date_created": "2005-12-22T22:10:44Z",
                        "media_type": "image",
                        "description": "Newborn stars, hidden behind thick dust, are revealed in this image of a section of the Christmas Tree cluster from NASA Spitzer Space Telescope, created in joint effort between Spitzer infrared array camera and multiband imaging photometer instrument"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA03244/PIA03244~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13930/collection.json",
                "data": [
                    {
                        "title": "It Twins! Spitzer Finds Hidden Jet",
                        "center": "JPL",
                        "nasa_id": "PIA13930",
                        "description_508": "NASA Spitzer Space Telescope took this image of a baby star sprouting two identical jets green lines emanating from fuzzy star. The left jet was hidden behind a dark cloud, which Spitzer can see through.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2011-04-04T22:30:02Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope took this image of a baby star sprouting two identical jets green lines emanating from fuzzy star. The left jet was hidden behind a dark cloud, which Spitzer can see through."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13930/PIA13930~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13303/collection.json",
                "data": [
                    {
                        "title": "Beastly Stars and a Bubble",
                        "center": "JPL",
                        "nasa_id": "PIA13303",
                        "description_508": "A star-forming region shines from the considerable distance of more than 30,000 light-years away in the upper left of this image from NASA Spitzer Space Telescope. This image is a combination of data from Spitzer and the Two Micron All Sky Survey.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/2MASS/SSI/University of Wisconsin",
                        "date_created": "2010-07-28T21:07:46Z",
                        "media_type": "image",
                        "description": "A star-forming region shines from the considerable distance of more than 30,000 light-years away in the upper left of this image from NASA Spitzer Space Telescope. This image is a combination of data from Spitzer and the Two Micron All Sky Survey."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13303/PIA13303~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12151/collection.json",
                "data": [
                    {
                        "title": "Coiled Creature of the Night",
                        "center": "JPL",
                        "nasa_id": "PIA12151",
                        "description_508": "NASA Spitzer Space Telescope has imaged a wild creature of the dark -- a coiled galaxy with an eye-like object at its center.",
                        "keywords": [
                            "NGC 1097",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2009-07-23T17:00:14Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has imaged a wild creature of the dark -- a coiled galaxy with an eye-like object at its center."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12151/PIA12151~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09962/collection.json",
                "data": [
                    {
                        "title": "Spitzer Celebrates Fourth Anniversary with Celestial Fireworks",
                        "center": "JPL",
                        "nasa_id": "PIA09962",
                        "description_508": "A newly expanded image of the Helix nebula lends a festive touch to the fourth anniversary of the launch of NASA Spitzer Space Telescope",
                        "keywords": [
                            "Helix Nebula",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2007-08-24T17:19:19Z",
                        "media_type": "image",
                        "description": "A newly expanded image of the Helix nebula lends a festive touch to the fourth anniversary of the launch of NASA Spitzer Space Telescope"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09962/PIA09962~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08509/collection.json",
                "data": [
                    {
                        "title": "Galaxies Gather at Great Distances",
                        "center": "JPL",
                        "nasa_id": "PIA08509",
                        "description_508": "Astronomers have discovered nearly 300 galaxy clusters and groups, including almost 100 located 8 to 10 billion light-years away, using the space-based Spitzer Space Telescope and the ground-based Mayall 4-meter telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/NOAO",
                        "date_created": "2006-06-05T16:55:30Z",
                        "media_type": "image",
                        "description": "Astronomers have discovered nearly 300 galaxy clusters and groups, including almost 100 located 8 to 10 billion light-years away, using the space-based Spitzer Space Telescope and the ground-based Mayall 4-meter telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08509/PIA08509~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11227/collection.json",
                "data": [
                    {
                        "title": "Stellar Work of Art",
                        "center": "JPL",
                        "nasa_id": "PIA11227",
                        "description_508": "This painterly portrait of a star-forming cloud, called NGC 346, is a combination of multiwavelength light from NASA Spitzer Space Telescope, the European Southern Observatory New Technology Telescope, and the European Space Agency.",
                        "keywords": [
                            "NGC 346",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/ESA/ESO/MPIA",
                        "date_created": "2008-10-08T17:46:41Z",
                        "media_type": "image",
                        "description": "This painterly portrait of a star-forming cloud, called NGC 346, is a combination of multiwavelength light from NASA Spitzer Space Telescope, the European Southern Observatory New Technology Telescope, and the European Space Agency."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11227/PIA11227~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13288/collection.json",
                "data": [
                    {
                        "title": "Space Balls Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA13288",
                        "description_508": "NASA Spitzer Space Telescope has at last found buckyballs resembling soccer balls in space shown in this artist concept using Hubble picture of the NGC 2440 nebula. Hubble image cred: NASA, ESA, STScI",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2010-07-22T17:59:59Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope has at last found buckyballs resembling soccer balls in space shown in this artist concept using Hubble picture of the NGC 2440 nebula. Hubble image cred: NASA, ESA, STScI"
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13288/PIA13288~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA02180/collection.json",
                "data": [
                    {
                        "title": "Galactic Hearts of Glass  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA02180",
                        "description_508": "This artist concept based on data fromNASA Spitzer Space Telescope shows delicate greenish crystals sprinkled throughout the violent core of a pair of colliding galaxies. The white spots represent a thriving population of stars of all sizes and ages.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "Artist Concept: NASA/JPL-Caltech<br />Graph: NASA/JPL-Caltech/Cornell",
                        "date_created": "2006-02-15T17:50:51Z",
                        "media_type": "image",
                        "description": "This artist concept based on data fromNASA Spitzer Space Telescope shows delicate greenish crystals sprinkled throughout the violent core of a pair of colliding galaxies. The white spots represent a thriving population of stars of all sizes and ages."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA02180/PIA02180~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15819/collection.json",
                "data": [
                    {
                        "title": "Cepheids as Cosmology Tools",
                        "center": "JPL",
                        "nasa_id": "PIA15819",
                        "description_508": "This graph illustrates the Cepheid period-luminosity relationship, used to calculate the size, age and expansion rate of the universe. The data shown are from NASA Spitzer Space Telescope which has made the most precise measurements yet.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2012-10-03T17:15:14Z",
                        "media_type": "image",
                        "description": "This graph illustrates the Cepheid period-luminosity relationship, used to calculate the size, age and expansion rate of the universe. The data shown are from NASA Spitzer Space Telescope which has made the most precise measurements yet."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15819/PIA15819~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17996/collection.json",
                "data": [
                    {
                        "title": "GLIMPSE the Galaxy All the Way Around",
                        "center": "JPL",
                        "nasa_id": "PIA17996",
                        "description_508": "This mosaic reveals a panorama of the Milky Way from NASA Spitzer Space Telescope. This picture covers only about three percent of the sky, but includes more than half of the galaxy stars and the majority of its star formation activity.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/University of Wisconsin",
                        "date_created": "2014-03-20T19:55:22Z",
                        "media_type": "image",
                        "description": "This mosaic reveals a panorama of the Milky Way from NASA Spitzer Space Telescope. This picture covers only about three percent of the sky, but includes more than half of the galaxy stars and the majority of its star formation activity."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17996/PIA17996~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18010/collection.json",
                "data": [
                    {
                        "title": "Mapping the Densest Dusty Cloud Cores",
                        "center": "JPL",
                        "nasa_id": "PIA18010",
                        "description_508": "Astronomers have found cosmic clumps so dark, dense and dusty that they throw the deepest shadows ever recorded. A large cloud looms in the center of this image of the galactic plane from NASA Spitzer Space Telescope.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/University of Zurich",
                        "date_created": "2014-05-21T17:55:41Z",
                        "media_type": "image",
                        "description": "Astronomers have found cosmic clumps so dark, dense and dusty that they throw the deepest shadows ever recorded. A large cloud looms in the center of this image of the galactic plane from NASA Spitzer Space Telescope."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18010/PIA18010~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13494/collection.json",
                "data": [
                    {
                        "title": "Weird Warm Spot on Exoplanet",
                        "center": "JPL",
                        "nasa_id": "PIA13494",
                        "description_508": "This frame from an animation based on NASA Spitzer Space Telescope data illustrates an unexpected warm spot on the surface of a gaseous exoplanet.The bright orange patches are the hottest part of the planet.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2010-10-19T17:00:53Z",
                        "media_type": "image",
                        "description": "This frame from an animation based on NASA Spitzer Space Telescope data illustrates an unexpected warm spot on the surface of a gaseous exoplanet.The bright orange patches are the hottest part of the planet."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13494/PIA13494~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10119/collection.json",
                "data": [
                    {
                        "title": "Baby Picture of our Solar System",
                        "center": "JPL",
                        "nasa_id": "PIA10119",
                        "description_508": "A rare, infrared view of a developing star and its flaring jets taken by NASA Spitzer Space Telescope shows us what our own solar system might have looked like billions of years ago. ",
                        "keywords": [
                            "L1157",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "IRAC image: NASA/JPL-Caltech/UIUC<br />  Visible image: NASA/JPL-Caltech/AURA",
                        "date_created": "2007-11-29T17:55:23Z",
                        "media_type": "image",
                        "description": "A rare, infrared view of a developing star and its flaring jets taken by NASA Spitzer Space Telescope shows us what our own solar system might have looked like billions of years ago. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10119/PIA10119~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11047/collection.json",
                "data": [
                    {
                        "title": "Spitzer Reveals Stellar Family Tree",
                        "center": "JPL",
                        "nasa_id": "PIA11047",
                        "description_508": "Generations of stars can be seen in this new infrared portrait from NASA Spitzer Space Telescope. In this wispy star-forming region, called W5, the oldest stars can be seen as blue dots in the centers of the two hollow cavities.",
                        "keywords": [
                            "W5",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/Harvard-Smithsonian CfA",
                        "date_created": "2008-08-22T16:04:54Z",
                        "media_type": "image",
                        "description": "Generations of stars can be seen in this new infrared portrait from NASA Spitzer Space Telescope. In this wispy star-forming region, called W5, the oldest stars can be seen as blue dots in the centers of the two hollow cavities."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11047/PIA11047~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11213/collection.json",
                "data": [
                    {
                        "title": "Supernova Flashback",
                        "center": "JPL",
                        "nasa_id": "PIA11213",
                        "description_508": "The Cassiopeia A supernova first flash of radiation makes six clumps of dust circled in annotated version unusually hot. The supernova remnant is the large white ball in the center. This infrared picture was taken by NASA Spitzer Space Telescope. ",
                        "keywords": [
                            "Cassiopeia A",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/E. Dwek and R. Arendt",
                        "date_created": "2008-10-01T16:14:27Z",
                        "media_type": "image",
                        "description": "The Cassiopeia A supernova first flash of radiation makes six clumps of dust circled in annotated version unusually hot. The supernova remnant is the large white ball in the center. This infrared picture was taken by NASA Spitzer Space Telescope. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11213/PIA11213~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08453/collection.json",
                "data": [
                    {
                        "title": "The Almost Invisible Aftermath of a Massive Star Death",
                        "center": "JPL",
                        "nasa_id": "PIA08453",
                        "description_508": "NASA Spitzer Space Telescope caught a glimpse of the Cepheus constellation, thirty thousand light-years away; astronomers think theyve found a massive star whose death barely made a peep. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/NASA Herschel Science Center/DSS",
                        "date_created": "2006-05-11T18:06:36Z",
                        "media_type": "image",
                        "description": "NASA Spitzer Space Telescope caught a glimpse of the Cepheus constellation, thirty thousand light-years away; astronomers think theyve found a massive star whose death barely made a peep. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08453/PIA08453~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08533/collection.json",
                "data": [
                    {
                        "title": "Supernova Dust Factory in M74",
                        "center": "JPL",
                        "nasa_id": "PIA08533",
                        "description_508": "Astronomers using NASA Spitzer Space Telescope have spotted a dust factory 30 million light-years away in the spiral galaxy M74. The factory is located at the scene of a massive star explosive death, or supernova. ",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech/STScI",
                        "date_created": "2006-06-09T21:15:22Z",
                        "media_type": "image",
                        "description": "Astronomers using NASA Spitzer Space Telescope have spotted a dust factory 30 million light-years away in the spiral galaxy M74. The factory is located at the scene of a massive star explosive death, or supernova. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08533/PIA08533~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09263/collection.json",
                "data": [
                    {
                        "title": "The Seven Sisters Pose for Spitzer",
                        "center": "JPL",
                        "nasa_id": "PIA09263",
                        "description_508": "The Seven Sisters, also known as the Pleiades star cluster, seem to float on a bed of feathers in a new infrared image from NASA Spitzer Space Telescope. Clouds of dust sweep around the stars, swaddling them in a cushiony veil. ",
                        "keywords": [
                            "Pleiades",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2007-04-16T16:19:07Z",
                        "media_type": "image",
                        "description": "The Seven Sisters, also known as the Pleiades star cluster, seem to float on a bed of feathers in a new infrared image from NASA Spitzer Space Telescope. Clouds of dust sweep around the stars, swaddling them in a cushiony veil. "
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09263/PIA09263~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18847/collection.json",
                "data": [
                    {
                        "title": "Ring of Stellar Fire",
                        "center": "JPL",
                        "nasa_id": "PIA18847",
                        "description_508": "This image from NASA Spitzer Space Telescope shows where the action is taking place in galaxy NGC 1291. The outer ring, colored red, is filled with new stars that are igniting and heating up dust that glows with infrared light.",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2014-10-22T16:00:13Z",
                        "media_type": "image",
                        "description": "This image from NASA Spitzer Space Telescope shows where the action is taking place in galaxy NGC 1291. The outer ring, colored red, is filled with new stars that are igniting and heating up dust that glows with infrared light."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18847/PIA18847~thumb.jpg",
                        "render": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA01902/collection.json",
                "data": [
                    {
                        "title": "Order Amidst Chaos of Star Explosion  Artist Concept",
                        "center": "JPL",
                        "nasa_id": "PIA01902",
                        "description_508": "This artist concept shows the explosion of a massive star, the remains of which are named Cassiopeia A. NASA Spitzer Space Telescope found evidence that the star exploded with some degree of order.",
                        "keywords": [
                            "Cassiopeia A",
                            "Spitzer Space Telescope"
                        ],
                        "secondary_creator": "NASA/JPL-Caltech",
                        "date_created": "2006-10-26T17:57:51Z",
                        "media_type": "image",
                        "description": "This artist concept shows the explosion of a massive star, the remains of which are named Cassiopeia A. NASA Spitzer Space Telescope found evidence that the star exploded with some degree of order."
                    }
                ],
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA01902/PIA01902~thumb.jpg",
                        "render": "image"
                    }
                ]
            }
        ],
        "metadata": {
            "total_hits": 508
        },
        "version": "1.0",
        "href": "https://images-api.nasa.gov/search?q=spitzer%20space%20telescope&keywords=spitzer%20space%20telescope&page=1",
        "links": [
            {
                "rel": "next",
                "prompt": "Next",
                "href": "https://images-api.nasa.gov/search?q=spitzer+space+telescope&page=2&keywords=spitzer+space+telescope"
            }
        ]
    }
}
             */
            var client = new RestClient("https://images-api.nasa.gov");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("search", Method.GET);
            request.AddParameter("q", "spitzer space telescope"); // adds to POST or URL querystring based on Method
            request.AddParameter("keywords", "spitzer space telescope"); // adds to POST or URL querystring based on Method

            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var response2 = client.Execute<NasaMediaLibrary>(request);
            library = response2.Data;
            if (library.Collection.Metadata.TotalHits > library.Collection.Items.Count)
            {
                Debug.WriteLine($"There is more data, TotalHits: {library.Collection.Metadata.TotalHits}, Items.Count: {library.Collection.Items.Count}");
                var pages = library.Collection.Metadata.TotalHits / library.Collection.Items.Count;
                Debug.WriteLine($"pages: {pages}");
                request.AddParameter("page", 0);
                for (int page = 2; page < pages; page++)
                {
                    Debug.WriteLine($"Loading page: {page} of {pages-1}");
                    request.Parameters[2].Value = page;
                    var pageResponse = client.Execute<NasaMediaLibrary>(request);
                    if (pageResponse.Data != null)
                    {
                        Debug.WriteLine($"Adding {pageResponse.Data.Collection.Items.Count} to library");
                        library.Collection.Items.AddRange(pageResponse.Data.Collection.Items);
                    }
                }
            }
        }

        public async Task<MediaItem> GetItemAsync(string id)
        {

            foreach(MediaItem item in library.Collection.Items)
            {
                if(item.Data.FirstOrDefault(d => d.NasaId == id) != null)
                {
                    return await Task.FromResult(item);
                }
            }
            return await Task.FromResult<MediaItem>(null);
        }

        public async Task<IEnumerable<MediaItem>> GetItemsAsync(bool forceRefresh = false)
        {
            if (library.Collection.Items != null)
            {
                return await Task.FromResult(library.Collection.Items.Select((MediaItem arg) => arg));
            }
            return await Task.FromResult<IEnumerable<MediaItem>>(null);
        }
    }
}
