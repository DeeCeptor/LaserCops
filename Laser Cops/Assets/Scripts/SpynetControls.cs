using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpynetControls : MonoBehaviour
{
    //the asteroid layout that is going to be spawned
    //each form will have a different selection of layouts to randomly choose one. more difficult layouts have a higher number
    public int layoutNumber;
    public List<GameObject> Layouts;
    //time between asteroid Spawns
    public float spawnDelay = 8f;
    public float spawnTimer = 0f;
    //wait slightly longer for the first spawn
    public float startOfGameDelay = 4f;
    // keeping track of which random layout was used last
    public int previousAttack;

    //form change conversations
    public ConversationManager formChangeConversation1;
    public ConversationManager formChangeConversation2;
    public ConversationManager formChangeConversation3;
    public ConversationManager formChangeConversation4;
    public ConversationManager formChangeConversation5;

    public int currentForm = 1;

    public BossHealthScript Health;
    public float healthThreshold = 0;
    public float healthBetweenStages = 50f;



    void Start ()
    {
        spawnTimer = spawnDelay + startOfGameDelay + Time.time;
        healthThreshold = Health.overallHealth - healthBetweenStages;
    }
	

	void FixedUpdate ()
    {
	    if(spawnTimer < Time.time)
        {
            SpawnAsteroid();
            spawnTimer = spawnDelay + Time.time;
        }

        if (Health.overallHealth < healthThreshold)
        {
            changeForms();
            healthThreshold = healthThreshold - healthBetweenStages;
        }
    }


    public void changeForms()
    {
        if (currentForm == 1)
        {
            if (GameState.game_state.current_difficulty > GameState.Difficulty.Easy)
                spawnDelay = 6.5f;

            PlayConversation(formChangeConversation1);
            currentForm = 2;
        }
        else if (currentForm == 2)
        {
            PlayConversation(formChangeConversation2);
            currentForm = 3;
        }
        else if (currentForm == 3)
        {
            if (GameState.game_state.current_difficulty > GameState.Difficulty.Easy)
                spawnDelay = 6f;

            PlayConversation(formChangeConversation3);
            currentForm = 4;
        }
        else if (currentForm == 4)
        {
            spawnDelay = 6f;
            PlayConversation(formChangeConversation4);
            currentForm = 5;
        }
        else if (currentForm == 5)
        {
            if (GameState.game_state.current_difficulty > GameState.Difficulty.Easy)
                spawnDelay = 5.3f;

            PlayConversation(formChangeConversation5);
            currentForm = 6;
        }
    }


    public void PlayConversation(ConversationManager conversation)
    {

        if (conversation != null)
        {
            conversation.transform.SetParent(null);
            conversation.Start_Conversation();
        }
    }


    public void SpawnAsteroid()
    {
        while (layoutNumber == previousAttack)
        {
            if (currentForm == 1)
            {
                layoutNumber = Random.Range(0, 4);
            }
            if (currentForm == 2)
            {
                layoutNumber = Random.Range(2, 6);
            }
            if (currentForm == 3)
            {
                layoutNumber = Random.Range(4, 8);
            }
            if (currentForm == 4)
            {
                layoutNumber = Random.Range(6, 10);
            }
            if (currentForm == 5)
            {
                layoutNumber = Random.Range(8, 12);
            }
            if (currentForm == 6)
            {
                layoutNumber = Random.Range(10, 14);
            }
        }

        previousAttack = layoutNumber;
        AsteroidMaterializerOnCall[] spawners = Layouts[layoutNumber].GetComponentsInChildren<AsteroidMaterializerOnCall>();
        for(int i = 0; i < spawners.Length;i++)
        {
            spawners[i].MaterializeAsteroid();
        }
    }

}
