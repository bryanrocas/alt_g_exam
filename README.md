# alt_g_exam


Bubble
- id : int	unique id
- color : enum	used to identify color sequence

Pattern Formation:
1) fired bubble id will be registered in array
on collision 2) use Physics.overlapCircleAll to detect neighbors
3) register neighbors to array IF id is not already registered AND it shares a color
4) newly reigstered bubbles will be pooled and will use (2) to identify and register neighbors
5) if no new bubbles are registered after (4), consider the pattern complete

Upon Pattern Completion:
1) count number of registered bubbles for chain count
2) chain count determines score
3) 'destroy' bubble chain and trigger falling of other bubbles

Bubble Behaviour:
1) balls should bounce/ricochet when fired
2) balls should stick when colliding with other balls
