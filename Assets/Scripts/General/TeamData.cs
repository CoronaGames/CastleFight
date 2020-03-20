using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamData : MonoBehaviour
{
    // Use to identify team belonging. And to make certain elements of Characters a corresponding color?
    [SerializeField] Team belongingToTeam;
    [SerializeField] SpriteRenderer miniMapRenderer;
    [SerializeField] SpriteRenderer flagRenderer;

    void Start()
    {
        SetColor(belongingToTeam);
    }

    public void SetTeamBelonging(Team team)
    {
        belongingToTeam = team;
        SetColor(team);
    }

    public Team GetTeamBelonging()
    {
        return belongingToTeam;
    }

    private void SetColor(Team team)
    {
        if (team == Team.None)
        {
            SetColor(Color.gray);
        }
        else if (team == Team.TeamBlue)
        {
            SetColor(Color.blue);
        }
        else if (team == Team.TeamGreen)
        {
            SetColor(Color.green);
        }
        else if (team == Team.TeamMagenta)
        {
            SetColor(Color.magenta);
        }
        else if (team == Team.TeamRed)
        {
            SetColor(Color.red);
        };
    }

    private void SetColor(Color color)
    {
        if (miniMapRenderer)
        {
            miniMapRenderer.color = color;
        }
        if (flagRenderer)
        {
            flagRenderer.color = color;
        }
    }

}
