﻿using System;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static event EventHandler<NextRaceEventArgs> NextRaceEvent;

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
            AddRaceTimes();
        }

        public static void ResetParticipant()
        {
            foreach (var participant in Competition.Participants)
            {
                participant.NumberOfLaps = -1;
                participant.StartTime = new DateTime();
                participant.Equipment.Speed = 20;
            }
        }

        public static void AddParticipants()
        {
            int speed = 20;
            int performance = 0;
            Driver d1 = new Driver("Michael", new Car(speed, performance), TeamColors.Blue);
            Driver d2 = new Driver("Sebastian", new Car(speed, performance), TeamColors.Green);
            Driver d3 = new Driver("Lewis", new Car(speed, performance), TeamColors.Red);
            Driver d4 = new Driver("Thomas", new Car(speed, performance), TeamColors.Grey);
            Driver d5 = new Driver("Albert", new Car(speed, performance), TeamColors.Yellow);
            Driver d6 = new Driver("Will", new Car(speed, performance), TeamColors.Yellow);

            Competition.Participants.Add(d1);
            Competition.Participants.Add(d2);
            Competition.Participants.Add(d3);
            Competition.Participants.Add(d4);
            Competition.Participants.Add(d5);
            Competition.Participants.Add(d6);
        }

        public static void AddTracks()
        {
            #region Tracks

            Track zwolle = new Track("Circuit Zwolle", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight
            });

            Track elburg = new Track("Circuit Elburg", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid,
                SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner,
                SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight
            });

            Track amsterdam = new Track("Rondje Amsterdam", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish,
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid
            });

            #endregion

            Competition.Tracks.Enqueue(elburg);
            Competition.Tracks.Enqueue(amsterdam);
            Competition.Tracks.Enqueue(zwolle);
        }

        public static void AddRaceTimes()
        {
            foreach (IParticipant participant in Competition.Participants)
            {
                Competition.RaceTimes.Add(participant, new TimeSpan());
            }
        }

        public static void ResetRaceTimes()
        {
            foreach (IParticipant participant in Competition.Participants)
            {
                Competition.RaceTimes[participant] = new TimeSpan();
            }
        }

        public static void NextRace()
        {
            CurrentRace?.Dispose();

            var tempTrack = Competition.NextTrack();

            if (tempTrack != null)
            {
                ResetParticipant();
                ResetRaceTimes();
                CurrentRace = new Race(tempTrack, Competition.Participants, Competition.RaceTimes);
                CurrentRace.RaceFinished += OnRaceFinished;
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs(CurrentRace));
                CurrentRace.StartTimer();
            }
            else
            {
                CurrentRace = null;
            }
        }

        public static void OnRaceFinished(object sender, EventArgs e)
        {
            NextRace();
        }
    }
}