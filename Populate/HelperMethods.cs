using System.Collections.Generic;
using Bogus;
using DataObjects;

namespace Populate {
    public static class HelperMethods {
        public static List<Engineer> getFakeEnginers() {
            var result = new List<Engineer>();
            for (int i = 0; i < 50; i++) {
                var engineerFab = new Faker<Engineer>()
                    .RuleFor(u => u.Name, (f, u) => f.Internet.UserName(u.Name))
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name));
                result.Add(engineerFab.Generate());
            }
            return result;
        }

        public static List<Partner> getFakePartners() {
            var result = new List<Partner>() {
                new Partner() {Name = "Rotiga"},
                new Partner() {Name = "Abisoft"},
                new Partner() {Name = "Electric Arts"},
                new Partner() {Name = "Passicevision"},
                new Partner() {Name = "Turn 99"},
                new Partner() {Name = "KOTAMI"},
                new Partner() {Name = "ROSS"},
                new Partner() {Name = "Aluminium Galaxy"},
                new Partner() {Name = "Capitan Vancouver"},
                new Partner() {Name = "Abisoft Mini"},
                new Partner() {Name = "Multiple"},
                new Partner() {Name = "Gearbox"},
                new Partner() {Name = "Cathead"},
                new Partner() {Name = "AFE"},
                new Partner() {Name = "Common"},
                new Partner() {Name = "Airos Montreal"},
                new Partner() {Name = "767 Industries"},
                new Partner() {Name = "Playground Games"  },
                new Partner() { Name = "FA"         },
                new Partner() {Name = "Capitan"            },
            };

            return result;
        }
        public static List<TopicFamily> getTopicFamilies() {
            var result = new List<TopicFamily>();

            result.Add(new TopicFamily() {
                Name = "Audio",
                Topics = new List<Topic>() {
                    new Topic() { Name = "Audio"}
                }
            });

            result.Add(new TopicFamily() {
                Name = "Graphics",
                Topics = new List<Topic>() {
                new Topic () { Name= "Topic 1"},
                new Topic () { Name= "Topic 2"},
                new Topic () { Name= "Performance"},
                new Topic () { Name= "Streaming"},
                }
            });

            result.Add(new TopicFamily()
            {
                Name = "Local",
                Topics = new List<Topic>() {
                new Topic () { Name= "Italy"},
                new Topic () { Name= "UK"},
                }
            });

            result.Add(new TopicFamily()
            {
                Name = "New senses",
                Topics = new List<Topic>() {
                new Topic () { Name= "Speech"},
                new Topic () { Name= "Vision"},
                new Topic () { Name= "Tactile"},
                new Topic () { Name= "Smell"},
                }
            });

            result.Add(new TopicFamily()
            {
                Name = "System & Tools",
                Topics = new List<Topic>() {
                new Topic() { Name= "Bootstrapping"},
                new Topic() { Name= "Performance End Points"},
                new Topic() { Name= "Performance Front end"},
                new Topic() { Name= "Performance Hardware"},
                new Topic() { Name= "Storage & Memory"},
                new Topic() { Name= "Test & Automation"},
                new Topic() { Name= "SDK"},
                new Topic() { Name= "IDE"},
                }
            });

            result.Add(new TopicFamily()
            {
                Name = "UI",
                Topics = new List<Topic>() {
                new Topic() { Name= "UI"},
                }
            });

            result.Add(new TopicFamily()
            {
                Name = "En vivo",
                Topics = new List<Topic>() {
                new Topic() { Name= "Stats"},
                new Topic() { Name= "Craig charles fund and sould show"},
                new Topic() { Name= "App certificate"}
                }
            });
            return result;
        }

    }
}
