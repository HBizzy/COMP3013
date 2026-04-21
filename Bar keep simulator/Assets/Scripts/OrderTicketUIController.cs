using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class OrderTicketUIController : MonoBehaviour
{
    public Dictionary<OrderTicket, GameObject> ticketObjectPairs = new Dictionary<OrderTicket, GameObject>();
    public List<OrderTicket> orderTickets = new List<OrderTicket>();
    public FeedbackManager feedbackManager;

    public AudioSource ticketSound;
    public AudioSource ticketFailSound;
    public GameObject ticketPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.orderManager.OnOrderGenerated += orderGenerated;
        GameStateManager.Instance.orderManager.OnOrderRemoved += removeTicket;
         
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        GameStateManager.Instance.orderManager.OnOrderGenerated -= orderGenerated;
        GameStateManager.Instance.orderManager.OnOrderRemoved -= removeTicket;
    }

    public void orderGenerated(OrderData drink)
    {
        if(orderTickets.Count < 4)
        {
            
            GameObject ticket = Instantiate(ticketPrefab,this.transform);
            ticket.GetComponent<OrderTicket>().Bind(drink);
            ticket.GetComponent<OrderTicket>().controller = this;
            orderTickets.Add(ticket.GetComponent<OrderTicket>());
            ticketObjectPairs.Add(ticket.GetComponent<OrderTicket>(), ticket);
        }
    }
    public void removeTicket(OrderData drink)
    {
        OrderTicket ticketToRemove = null;
        foreach (OrderTicket ticket in orderTickets)
        {
            if(ticket.Order == drink)
            {
                ticketToRemove = ticket;
            }
        }
        if(ticketToRemove != null)
        {
            Destroy(ticketObjectPairs[ticketToRemove]);
            ticketObjectPairs.Remove(ticketToRemove);
            orderTickets.Remove(ticketToRemove);
            Debug.Log("Order complete");
            PlayTicketFailSound();
        }
    }
    public void ResetAllTicketHighlights()
    {
        foreach(OrderTicket ticket in orderTickets)
        {
            ticket.ResetHighlight();
        }
    }
    public void PlayTicketSound()
    {
        ticketSound.pitch = UnityEngine.Random.RandomRange(0.9f, 1.1f);
        ticketSound.Play();
    }
    public void PlayTicketFailSound()
    {
        ticketFailSound.pitch = UnityEngine.Random.RandomRange(0.9f, 1.1f);
        ticketFailSound.Play();
    }
}
