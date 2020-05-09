using Godot;
using System;
using System.Collections.Generic;

public class Region : Node
{
    public int number;
    List<Chunk> chunks;
    public List<Quest> quests;
    Properties properties;
    Properties propertiesAfterQuest;

    public Region(int number)
    {
        this.number = number;
        if(number >= 0)
        {
            properties = new Properties();   
            propertiesAfterQuest = new Properties(properties);
        }
    }

    public void AddChunk(Chunk chunk)
    {
        if(chunks == null)
            chunks = new List<Chunk>();

        chunks.Add(chunk);
    }

    public void AddQuest(Quest quest)
    {
        if(quests == null)
            quests = new List<Quest>();

        quests.Add(quest);
    }

    public bool Equals(Region region)
    {
        if(this.number == region.number)
            return true;
        else
            return false;
    }

    public Quest NextQuest()
    {
        if(quests != null)
            for(int i = 0; i < quests.Count; i++)
                if(!quests[i].Done)
                    return quests[i];
        return null;
    }

    public void CompleteQuest()
    {
        if(NextQuest().Available())
            NextQuest().Complete(this);
        else
            GD.Print("Could not complete quest.");
    }
    public void ClearQuests()
    {
        if(quests == null)
            return;
        
        for(int i = 0; i < quests.Count; i++)
        {
            quests[i] = null;
        }
        quests.Clear();

    }

    public Properties Properties
    {
        get{return properties;}
    }

    public Properties PropertiesAfterQuest
    {
        get{return propertiesAfterQuest;}
    }
}