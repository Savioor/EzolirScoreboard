using EzolirScoreboard.MatchRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzolirScoreboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region addTeam

        private void addTeamButt_Click(object sender, EventArgs e)
        {
            uint teamNum = Decimal.ToUInt32(newTeamNum.Value);
            if (GlobalData.get.teams.Exists((team) => team.number == teamNum))
            {
                MessageBox.Show("Team #" + teamNum + " already exists", "Error");
                return;
            }
            String teamName = newTeamName.Text;
            if (GlobalData.get.teams.Exists((team) => team.name.Equals(teamName)))
            {
                MessageBox.Show("Team \"" + teamName + "\" already exists", "Error");
                return;
            }

            GlobalData.get.teams.Add(new Team(teamName, teamNum));

            MessageBox.Show("Team #" + teamNum + " \"" + teamName + "\" was added!", "Success");

            updateTeamView();
            GlobalData.get.display.updateScores();
        }

        private void updateTeamView()
        {
            teams.Items.Clear();
            String[] newRow;
            foreach (Team team in GlobalData.get.teams)
            {
                newRow = new String[5];
                newRow[0] = "I" + team.number.ToString();
                newRow[1] = team.name;
                newRow[2] = team.score.ToString();
                newRow[3] = team.gatePoints.ToString();
                newRow[4] = team.bonusPoints.ToString();
                teams.Items.Add(new ListViewItem(newRow));
            }
        }

        private void removeMarked_Click(object sender, EventArgs e)
        {
            String name;
            foreach (ListViewItem item in teams.Items)
            {
                if (item.Checked)
                {
                    name = item.SubItems[1].Text;
                    GlobalData.get.teams.Remove(GlobalData.get.getTeamByName(name));
                }
            }

            updateTeamView();

            MessageBox.Show("Teams removed", "Success");
        }

        #endregion

        private void button1_Click(object sender, EventArgs e) {}

        private void updateMatchTeamView()
        {
            uint counter = 0;
            MatchAdder adder = MatchAdder.get;
            teamsForMatch.Items.Clear();
            string[] obj;
            if (!isDoubleGame())
            {
                foreach (Match.Participant p in adder.regularGame)
                {
                    obj = new string[3];
                    counter++;
                    obj[0] = p.getFirstTeam().number.ToString();
                    obj[1] = p.getFirstTeam().name;
                    obj[2] = counter.ToString();
                    teamsForMatch.Items.Add(new ListViewItem(obj));
                }
            }
            else
            {
                foreach (Match.Participant p in adder.doubleGame)
                {
                    obj = new string[3];
                    counter++;
                    obj[0] = p.getFirstTeam().number.ToString() + "&" + p.getSecondTeam().number.ToString();
                    obj[1] = p.getFirstTeam().name + "&" + p.getSecondTeam().name;
                    obj[2] = counter.ToString();
                    teamsForMatch.Items.Add(new ListViewItem(obj));
                }
            }
        }

        private bool isDoubleGame() { return Decimal.ToInt32(matchTrack.Value) == 3; }

        private void updateTeamPickers()
        {
            teaamOnePicker.Items.Clear();
            teaamOnePicker.Text = "";
            teamTwoPicker.Items.Clear();
            teamTwoPicker.Text = "";
            bool isGay;
            foreach (Team team in GlobalData.get.teams)
            {
                isGay = false;
                foreach(ListViewItem gay in teamsForMatch.Items)
                {
                    Console.WriteLine(gay.SubItems[0]);
                    if (!isDoubleGame())
                    {
                        if (gay.SubItems[1].Text.Equals(team.name))
                        {
                            isGay = true;
                            break;
                        }
                    }
                    else
                    {
                        if (gay.SubItems[1].Text.Split('&')[0].Equals(team.name)
                            || gay.SubItems[1].Text.Split('&')[1].Equals(team.name))
                        {
                            isGay = true;
                            break;
                        }
                    }
                }
                if (!isGay)
                {
                    teaamOnePicker.Items.Add(team.number.ToString());
                    teamTwoPicker.Items.Add(team.number.ToString());
                }
            }
        }

        private void matchTrack_ValueChanged(object sender, EventArgs e)
        {
            updateMatchTeamView();
            updateTeamPickers();
            if (isDoubleGame())
            {
                addGroupToMatch.Enabled = true;
                addTeamToMatch.Enabled = false;
                teamTwoPicker.Enabled = true;
            }
            else
            {
                addGroupToMatch.Enabled = false;
                addTeamToMatch.Enabled = true;
                teamTwoPicker.Enabled = false;
            }
        }

        private void addTeamToMatch_Click(object sender, EventArgs e)
        {
            try
            {
                string teamNum = teaamOnePicker.Text;
                Team toAdd = GlobalData.get.getTeamByNumber(UInt32.Parse(teamNum));
                MatchAdder.get.regularGame.Add(new Match.Participant(toAdd));
                updateMatchTeamView();
                updateTeamPickers();
            } catch(FormatException) { }
        }

        private void addGroupToMatch_Click(object sender, EventArgs e)
        {
            try
            {
                string teamNum1 = teaamOnePicker.Text;
                string teamNum2 = teamTwoPicker.Text;
                Team toAdd1 = GlobalData.get.getTeamByNumber(UInt32.Parse(teamNum1));
                Team toAdd2 = GlobalData.get.getTeamByNumber(UInt32.Parse(teamNum2));
                MatchAdder.get.doubleGame.Add(new Match.Participant(toAdd1, t2:toAdd2));
                updateMatchTeamView();
                updateTeamPickers();
            }
            catch (FormatException) { }
        }

        private void removedCheckedFromMatch_Click(object sender, EventArgs e)
        {
            String name;
            foreach (ListViewItem item in teamsForMatch.Items)
            {
                if (item.Checked)
                {
                    name = item.SubItems[1].Text.Split('&')[0];
                    if (isDoubleGame())
                    {
                        MatchAdder.get.doubleGame.Remove(MatchAdder.get.doubleGame.Find((a) => a.getFirstTeam().name == name));
                    }
                    else
                    {
                        MatchAdder.get.regularGame.Remove(MatchAdder.get.regularGame.Find((a) => a.getFirstTeam().name == name));
                    }
                }
            }

            updateMatchTeamView();
            updateTeamPickers();
        }

        private void addTeamsTest1_Click(object sender, EventArgs e)
        {

            for (uint i = 1000; i <= 1010; i++)
            {
                GlobalData.get.teams.Add(new Team(i.ToString(), i));
            }

            updateTeamView();
            GlobalData.get.display.updateScores();
            addTeamsTest1.Enabled = false;
        }

        private void updateRegisterdMatches()
        {
            registeredMatches.Items.Clear();
            string[] obj;
            foreach(Match match in GlobalData.get.matches)
            {
                obj = new string[4];
                obj[0] = match.uID.ToString();
                obj[1] = match.trackID.ToString();
                obj[2] = match.participants[0].getFirstTeam().number.ToString(); // TODO (this is SHIIIT) ok slightly better
                obj[3] = match.areScoresSet() ? match.getWinner().ToString() : "Scores not set";

                registeredMatches.Items.Add(new ListViewItem(obj));
            }
        }

        private void addMatch_Click(object sender, EventArgs e)
        {
            if (teamsForMatch.Items.Count == 0)
                return;
            if (isDoubleGame())
            {
                GlobalData.get.addNewMatch(MatchAdder.get.doubleGame, 3);
                MatchAdder.get.restartDouble();
            }
            else
            {
                GlobalData.get.addNewMatch(MatchAdder.get.regularGame, Decimal.ToUInt32(matchTrack.Value));
                MatchAdder.get.restartRegular();
            }
            updateMatchTeamView();
            updateTeamPickers();
            updateRegisterdMatches();
        }

        private void updateRegisteredMatches_Tick(object sender, EventArgs e)
        {
            Match curr;
            Team currTeam;
            Match.Participant par;
            foreach (ListViewItem itm in registeredMatches.Items)
            {
                curr = GlobalData.get.getMatchByUId(Int32.Parse(itm.SubItems[0].Text));
                currTeam = GlobalData.get.getTeamByNumber(UInt32.Parse(itm.SubItems[2].Text.Split('&')[0]));
                par = curr.participants[(curr.participants.FindIndex((p) => p.getFirstTeam().number == currTeam.number) + 1) % curr.participants.Count];
                itm.SubItems[2].Text = par.ToString();
            }
        }

        // Oh shit fucking button names in C#, this is the match removing button
        private void button2_Click(object sender, EventArgs e)
        {
            Match curr;
            foreach(ListViewItem itm in registeredMatches.Items)
            {
                if (itm.Checked)
                {
                    curr = GlobalData.get.getMatchByUId(Int32.Parse(itm.SubItems[0].Text));
                    GlobalData.get.matches.Remove(curr);
                }
            }
            updateRegisterdMatches();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // XD I HATE ACCIDENTLY PRESSING SHIT
        }

        private void getData_Click(object sender, EventArgs e)
        {
            try
            {
                Match match = GlobalData.get.getMatchByUId(Decimal.ToInt32(matchSetter.Value));
                matchToScore.Text = match.uID.ToString();
                team1.Text = match.participants[0].labelToString();
                team2.Text = match.participants[1].labelToString();
                team3.Text = match.participants[2].labelToString();
                if (match.areScoresSet())
                {
                    placeTeam1.Value = match.participants[0].score.place;
                    minTeam1.Value = match.participants[0].score.time.Minutes;
                    secTeam1.Value = match.participants[0].score.time.Seconds;
                    miliTeam1.Value = match.participants[0].score.time.Milliseconds;
                    gatesTeam1.Value = match.participants[0].score.gates;

                    placeTeam2.Value = match.participants[1].score.place;
                    minTeam2.Value = match.participants[1].score.time.Minutes;
                    secTeam2.Value = match.participants[1].score.time.Seconds;
                    miliTeam2.Value = match.participants[1].score.time.Milliseconds;
                    gatesTeam2.Value = match.participants[1].score.gates;

                    placeTeam3.Value = match.participants[2].score.place;
                    minTeam3.Value = match.participants[2].score.time.Minutes;
                    secTeam3.Value = match.participants[2].score.time.Seconds;
                    miliTeam3.Value = match.participants[2].score.time.Milliseconds;
                    gatesTeam3.Value = match.participants[2].score.gates;
                }
                else
                {
                    placeTeam1.Value = 0;
                    minTeam1.Value = 0;
                    secTeam1.Value = 0;
                    miliTeam1.Value = 0;
                    gatesTeam1.Value = 0;

                    placeTeam2.Value = 0;
                    minTeam2.Value = 0;
                    secTeam2.Value = 0;
                    miliTeam2.Value = 0;
                    gatesTeam2.Value = 0;

                    placeTeam3.Value = 0;
                    minTeam3.Value = 0;
                    secTeam3.Value = 0;
                    miliTeam3.Value = 0;
                    gatesTeam3.Value = 0;
                }
            }
            catch { }
        }

        private void setData_Click(object sender, EventArgs e)
        {
            Match match = GlobalData.get.getMatchByUId(Int32.Parse(matchToScore.Text));
            List<ScoresObject> toSet = new List<ScoresObject>();

            toSet.Add(new ScoresObject(
                Decimal.ToUInt32(placeTeam1.Value),
                Decimal.ToUInt32(gatesTeam1.Value),
                new TimeSpan(days:0, hours:0, minutes:Decimal.ToInt32(minTeam1.Value), seconds:Decimal.ToInt32(secTeam1.Value), milliseconds:Decimal.ToInt32(miliTeam1.Value))
                ));

            toSet.Add(new ScoresObject(
                Decimal.ToUInt32(placeTeam2.Value),
                Decimal.ToUInt32(gatesTeam2.Value),
                new TimeSpan(days: 0, hours: 0, minutes: Decimal.ToInt32(minTeam2.Value), seconds: Decimal.ToInt32(secTeam2.Value), milliseconds: Decimal.ToInt32(miliTeam2.Value))
                ));

            toSet.Add(new ScoresObject(
                Decimal.ToUInt32(placeTeam3.Value),
                Decimal.ToUInt32(gatesTeam3.Value),
                new TimeSpan(days: 0, hours: 0, minutes: Decimal.ToInt32(minTeam3.Value), seconds: Decimal.ToInt32(secTeam3.Value), milliseconds: Decimal.ToInt32(miliTeam3.Value))
                ));

            match.setScores(toSet);

            updateRegisterdMatches();
            GlobalData.get.display.updateScores();
        }

        private void addMatches_Click(object sender, EventArgs e)
        {
            // XD TODO
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                GlobalData.get.display.Activate();
                GlobalData.get.display.scoresUpdater.Enabled = true;
                GlobalData.get.display.Show();
                GlobalData.get.display.updateScores();
            } catch (ObjectDisposedException)
            {
                GlobalData.get.recreateDisplay();
                GlobalData.get.display.Activate();
                GlobalData.get.display.scoresUpdater.Enabled = true;
                GlobalData.get.display.Show();
                GlobalData.get.display.updateScores();
            }
        }

        private void removeBonus_Click(object sender, EventArgs e)
        {
            foreach(Match m in GlobalData.get.matches)
            {
                if(m.trackID == Decimal.ToUInt32(matchToBonus.Value))
                {
                    foreach(Match.Participant p in m.participants)
                    {
                        p.getFirstTeam().bonusPoints = 0;
                        if (m.trackID == 3)
                        {
                            p.getSecondTeam().bonusPoints = 0;
                        }
                    }
                }
            }
        }

        private void bonusAdd_Click(object sender, EventArgs e)
        {
            List<Match.Participant> teams = new List<Match.Participant>();
            foreach (Match m in GlobalData.get.matches)
            {
                if (m.trackID == Decimal.ToUInt32(matchToBonus.Value))
                {
                    teams.AddRange(m.participants);
                }
            }
            teams.RemoveAll((a) => a.score.time.TotalMilliseconds == 0);
            teams.OrderBy((a) => a.score.time.TotalMilliseconds);
            try
            {
                if (Decimal.ToUInt32(matchToBonus.Value) == 3)
                {
                    teams[0].getFirstTeam().bonusPoints = 15;
                    teams[0].getSecondTeam().bonusPoints = 15;
                    teams[1].getFirstTeam().bonusPoints = 10;
                    teams[1].getSecondTeam().bonusPoints = 10;
                    teams[2].getFirstTeam().bonusPoints = 5;
                    teams[2].getSecondTeam().bonusPoints = 5;
                }
                else
                {
                    teams[0].getFirstTeam().bonusPoints = 15;
                    teams[1].getFirstTeam().bonusPoints = 10;
                    teams[2].getFirstTeam().bonusPoints = 5;
                }
            }
            catch (IndexOutOfRangeException) { }
        }

        private void hideScores_Click(object sender, EventArgs e)
        {
            GlobalData.get.display.scoresUpdater.Enabled = false;
            GlobalData.get.display.Hide();
        }
    }
}
