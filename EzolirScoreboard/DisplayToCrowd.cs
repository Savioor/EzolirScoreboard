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
    public partial class DisplayToCrowd : Form
    {

        private Label[] place;
        private Label[] team;
        private Label[] number;
        private Label[] total;
        private Label[] track1;
        private Label[] track2;
        private Label[] track3;
        private Label[] bonus;
        private GroupBox[] boxes;

        public DisplayToCrowd()
        {
            InitializeComponent();
            place = new Label[] { place1, place2, place3, place4, place5, place6, place7, place8 };
            team = new Label[] { team1, team2, team3, team4, team5, team6, team7, team8 };
            number = new Label[] { number1, number2, number3, number4, number5, number6, number7, number8 };
            total = new Label[] { total1, total2, total3, total4, total5, total6, total7, total8 };
            track1 = new Label[] { track11, track12, track13, track14, track15, track16, track17, track18 };
            track2 = new Label[] { track21, track22, track23, track24, track25, track26, track27, track28 };
            track3 = new Label[] { track31, track32, track33, track34, track35, track36, track37, track38 };
            bonus = new Label[] { bonus1, bonus2, bonus3, bonus4, bonus5, bonus6, bonus7, bonus8 };
            boxes = new GroupBox[] { groupBox1, groupBox2, groupBox3, groupBox4, groupBox5, groupBox6, groupBox7, groupBox8 };

            int width = Screen.PrimaryScreen.Bounds.Width;
            int boxWidth = width - (width / 10);
            int boxOver20 = boxWidth / 20;

            for(int i = 0; i < 8; i++)
            {
                boxes[i].Top = (Screen.PrimaryScreen.Bounds.Height / 26) * (3 * i + 2);
                boxes[i].Left = (width / 20);
                boxes[i].Width = boxWidth;
                place[i].Left = boxOver20 / 5;
                team[i].Left = (int)(boxOver20 * 1.3);
                number[i].Left = boxOver20 * 6;
                total[i].Left = boxOver20 * 10;
                track1[i].Left = boxOver20 * 12;
                track2[i].Left = boxOver20 * 14;
                track3[i].Left = boxOver20 * 16;
                bonus[i].Left = boxOver20 * 18;
            }

            hPlace.Left = (width / 20);
            hName.Left = (width / 20) + (int)(boxOver20 * 1.3);
            hNumber.Left = (width / 20) + boxOver20 * 6;
            hTotal.Left = (width / 20) + boxOver20 * 10;
            hTrack1.Left = (width / 20) + boxOver20 * 12;
            hTrack2.Left = (width / 20) + boxOver20 * 14;
            hScores.Left = (width / 20) + boxOver20 * 14;
            hTrack3.Left = (width / 20) + boxOver20 * 16;
            hBonus.Left = (width / 20) + boxOver20 * 18;

        }
        // OOOPS
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        private int cycle = 0;
        private bool cycleOver = false;

        private void scoresUpdater_Tick(object sender, EventArgs e)
        {
            List<Team> rankings = GlobalData.get.getRankedTeams();
            for(int i = cycle*8; i < (cycle + 1)*8; i++)
            {
                place[i % 8].Text = (i + 1).ToString();
                if (i >= rankings.Count)
                {
                    team[i % 8].Text = "N/A";
                    number[i % 8].Text = "N/A";
                    total[i % 8].Text = "N/A";
                    track1[i % 8].Text = "N/A";
                    track2[i % 8].Text = "N/A";
                    track3[i % 8].Text = "N/A";
                    bonus[i % 8].Text = "N/A";
                    cycleOver = true;
                }
                else
                {
                    team[i % 8].Text = rankings[i].name;
                    number[i % 8].Text = "I" +  rankings[i].number.ToString();
                    total[i % 8].Text = rankings[i].score.ToString();
                    try
                    {
                        track1[i % 8].Text = ((4 - rankings[i].appearIn.Find((a) => a.trackID == 1).getParticipantsByTeam(rankings[i]).score.place) * 5).ToString();
                    } catch (NullReferenceException)
                    {
                        track1[i % 8].Text = "0";
                    }
                    try
                    {
                        track2[i % 8].Text = ((4 - rankings[i].appearIn.Find((a) => a.trackID == 2).getParticipantsByTeam(rankings[i]).score.place) * 5).ToString();
                    }
                    catch (NullReferenceException)
                    {
                        track2[i % 8].Text = "0";
                    }
                    try
                    {
                        track3[i % 8].Text = ((4 - rankings[i].appearIn.Find((a) => a.trackID == 3).getParticipantsByTeam(rankings[i]).score.place) * 5).ToString();
                    }
                    catch (NullReferenceException)
                    {
                        track3[i % 8].Text = "0";
                    }
                    bonus[i % 8].Text = rankings[i].bonusPoints.ToString();
                }
            }
            cycle++;
            if (cycleOver)
            {
                cycle = 0;
                cycleOver = false;
            }
        }

        // OOOPS 2
        private void label47_Click(object sender, EventArgs e)
        {

        }
    }
}
