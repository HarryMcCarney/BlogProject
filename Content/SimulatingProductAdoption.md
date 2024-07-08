---
category: Note
tags: Simulation, Data Science
updated: 20230224
created: 20230224
title: Simulating viral product adoption.
summary: This basic model shows how adoption can spread virally with non linear tipping points. Much like disinformation in social media networks and infectious disease in populations.
---

# Product Adoption Simulation
Rough simulation to illustrate the concept. 
View it [here](https://harrymccarney.github.io/ProductAdoptionSim/) 

You are launching a new product and want to know how much to spend on marketing.

You know that peer pressure is very effective so you just need to convert some early adopters and then they will convert further prospects leading to viral adoption. 

Each 5x5 grid below is a separate simulation instance. The default is to run 12 instances simultaneously and then observe the results and variance across them. Red dots are prospects and green dots have adopted the product. The dots move at random each tick from one cell to another. If they encounter n number of adopters in the cell they also become adopters. 

Starting values can be configured including Number of simulations, Peer Pressure Threshold (number of adopters needed in a cell to convert prospects),  Population size and Marketing spend (number of randomly placed adopters sim starts with)

Assuming peer pressure threshold of 3, the Inflexion point is reached efficiently within a population of 100 (density is important) and initial marketing spend of 7 adopters. 