---
category: Essay
tags: Agile, H&C, Software Engineering, Quality Control
updated: 20200516
created: 20200516
title: How Quality Control processes can undermine quality
summary: Software must now evolve faster than ever. Underlying technology such as cloud infrastructure, operating systems and programming languages now receive near continuous updates from Google, Microsoft, Amazon, and the open source community.
---
Competition between software providers has intensified. Increasing customer expectations require companies to rapidly ‘test and learn’ to find and maintain their markets. Most critically, the security landscape is generating serious new threats almost weekly that previously would occur monthly or annually. 

Many organisations, particularly in regulated industries, rely on release processes that were originally designed to maintain quality when the pace of the change was slower. As the need for updates speeds up, some have reached a tipping point and can no longer keep up. Their backlog of critical fixes expands exponentially and the release speed slows in unison. 

---

At H&C, we know that maintaining software through regular small updates is easier than large staged releases. Staged releases contain many simultaneous updates, which cause interdependencies that multiply the complexity.

Engineering managers in large organisations are understandably risk averse. They rightly place the burden of proof on the claim that leaner quality control processes are essential for creating quality. This article attempts to provde that proof and suggest how we can begin to design more effective quality control processes.

![Sisyphus](/Sisyphus.jpg)

## Why systems are unstable


Systems are a union of form and context.[1] As context is constantly evolving, complex systems can never be stable. The ‘form’ of a system refers to its set of formal rules and features. In a software system, this could be the business logic, user experience, interface design or algorithms. The ‘context’ refers to all factors that the form must interact with. In a software system, this includes the cloud environment, user requirements, technical proficiency of the user, connection speeds, current security threats and competitor products. 

The quality of a software system is a measure of the closeness of the fit between the form and its surrounding context. The challenge all systems face is the constant evolution of the context. In recent years the pace of this evolution has accelerated. 

These changes in context lead to misfits with the form. System maintenance must identify and patch these misfits so that the form evolves in tandem with the context. Due to the constantly evolving nature of the context this amounts to a Sisyphean game of ‘whack-a-mole’. 

## Complexity does not increase linearly

The effort required to resolve these misfits does not increase linearly with each new misfit. The interdependence between misfits in different areas of any system multiplies its complexity. Even if that interdependence is not strictly formal, the practical human effort to make multiple simultaneous changes to a system increases exponentially with the number of changes that need to be made.

Many developments within software engineering such as the single responsibility principle (SRP) and Microservice Architecture are designed to address this problem. The hope is that breaking the system down into small independent parts will prevent complexity buildups as misfits remain local to an individual component. However, microservices are subsystems that, by definition, have complex interdependencies, which require careful planning and simultaneous updates.

![ComplexityandLinkages](/SystemDebtDiagrams.png)


Figures 1-3 show the complexity increasing as the number of misfits and corresponding fixes increase. Each box represents a version of the system, similar to a code branch containing a fix for a specific misfit. A is the current production system and the other boxes are discrete versions. 

Simultaneous changes (these are necessary if the number of misfits increase), create exponential complexity as every branch must be compatible with every other. This is in contrast to Figure 4 in which the changes are made fast enough to avoid a build up and can therefore be made linearly. 

The cognitive load of addressing the complexity is a factor of the number of links in the release. The links are depicted as double ended arrows. Figure 3 has ten links between the components, while Figure 4 has just four. This can be formalised as C = L², where C is complexity and L is the number of links.

The relationship between complexity and interdepency in simultaneous updates creates the phenomenon that a linear rewrite of a system from scratch is often simpler than patching an existing system. This fact leads to difficult conversations between engineers and their managers who seldom understand the practical implications of this type of complexity.

## Release cycles must keep pace with context changes to prevent a build up in complexity

![Release cycle](/MisfitGraphs.png)

Figure 5 shows the different stages in a release cycle of a system. The length of 'Time to Live' is fixed by the organisation’s internal processes. Each stage contributes to the Time to Live, but in organisations that are struggling with quality issues, the ‘Fix Validated’ stage tends to increase. 

As release cycles lengthen they also become less elastic. This means the Time to Live takes approximately the same amount of time regardless of the scale of the fix currently being pushed through. 

The other pivotal interval in the diagram is the Misfit Frequency. This is the time between each context update that goes on to create a new misfit to be addressed. To prevent the build up in complexity, the Time to Live must be shorter than the Misfit Frequency. 

Misfit Frequency will vary widely from system to system. The printing presses changed very slowly until the information age. Now every blog platform must regularly and reactively release fixes for new versions of the major browsers. 

This step change in the evolution speed of context presents serious challenges to transforming organisations. Those that are moving from the relatively slow moving hardware medium to the more rapid software medium - or from installable software to the cloud - find their release processes are too slow to keep up with the context evolution and resulting misfit frequency. This leads to a build up of complexity that quickly swamps the organisation and erodes the quality of their systems.

## Expanding Quality Control processes is counter productive if the release cycle becomes too slow

An understandable reaction from management to seeing poor quality systems go into production is to question why there weren't more checks in place to prevent this happening. This often results in adding yet more layers of checks, rather than fine tuning of the existing ones. 

This leads to a proliferation of overlapping quality processes, which slow the speed at which misfits can be resolved. Eventually process overheads reach critical mass, pushing the project into a vicious circle of increasing complexity and bureaucratic paralysis.

Due to the nature of complexity, quality is directly correlated to how fast a system can get a misfit fix into production. Despite this, it is also clear that engineers do make mistakes and that some form of checking prior to production deployment is essential. 

Automated testing was developed as a response to the need for rapid large scale testing. In practice some form of manual testing should always be part of the fail safe. However, the utility of the manual process depends on both its accuracy in identifying detects and, crucially, the delay it creates in Time to Live.  (In a future post I will describe the techniques H&C uses to increase quality without lengthening release cycles. In short, they arise from the change of perspective which sees quality as an ongoing process rather than a retrospective spot check. [2] )

The accuracy of any validation stage can be formalised as a probability of detecting a defect if one is present. After a certain threshold of defect detection probability is achieved, any further expansion of validation processes gives diminishing returns, slows the Time to Live, builds up complexity, and thereby decreases quality.

## The quality of a system is determined by the ratio between the time it takes to deploy a fix, and the speed at which the context generates the need for new fixes

As we have seen, it is essential to avoid the simultaneous changes caused by a build up of unaddressed misfits. We can formulate this understanding of the relationship between quality, validation processes, and the time it takes to get a fix into production as follows:

Quality (Q): The closeness of the fit between form and context.\
Misfit Frequency (MF): The time interval between each misfit. \
Time to Live (TTL): TIme interval from discovery of misfit to deployment into production.\
Defect Detection: The probability (expressed as number between 0 and 1) of the quality control process uncovering a defect in a new release addressing a misfit.

Q = (MF / TTL) * DD 

While simplistic, this equation does give insight into the real relationship between these variables and shows how we can begin to optimise for quality. It shows that TTL must be less than MF in order to prevent the build up of misfits and the paralysing complexity that comes with it 

Sample calculations could include, where: 

TTL = 5 days\
MF = 8 days\
DD = 80% chance of spotting defects.

(8/5) * 0.8 = Quality 1.28

Critically, if we adjust these variables slightly to make the Time to Live longer at seven days, almost at a misfit rate, we get a quality score of 1.02. 

If the Time to Live exceeds the Misfit Frequency then quality very rapidly deteriorates. For example, where: 

TTL = 15 days\
MF = 8 days\
DD = 95% 

Gives a quality score of 0.5.

![Quality by Defect Dectection](/QualitybyDefectDetection.png)

This graph shows how quality decreases once the Defect Detection probability increases above a certain point. It assumes that each 10% increase in the DD rate adds one unit onto the Time to Live. It illustrates that lengthy release processes cause an exponential increase in complexity which will inevitably lead to a decrease in quality. 

 
## Quality control processes must be lean

A proper understanding of complexity and its root causes necessitates a release process faster than the rate at which the context evolves. Organisations must be sceptical of any increase in process weight that may obstruct their ability to deploy necessary fixes. The tendency towards process bloat is also self-perpetuating and difficult to reverse. 

The context in which all information systems operate is evolving faster than ever. External dependencies such as the cloud are updated on a near daily basis. User expectations of the power and simplicity of technology are also growing exponentially. Systems which are trapped in a vicious circle of bureaucracy and complexity buildups will fall further and further behind. 

We must create quality control processes that are as nimble and fast as they are robust and thorough. Indeed, quality control processes should be as concerned with the fixes they don't make as those that they do. 


##### Notes

[1] The nature of Form and Context is explored in more detail by Christopher Alexander in his book about Architectural Design [‘Notes on the Synthesis of Form.'](https://en.wikipedia.org/wiki/Notes_on_the_Synthesis_of_Form)

[2] H&C's approach includes embedding a Secure Development Lifecycle and threat modelling into our sprint cycles. We also use statically typed functional programming languages. Their algebraic type system increases domain model expressiveness and captures many errors at compile time before the code is deployed. 

