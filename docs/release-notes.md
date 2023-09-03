# Release Notes

## `0.7.0`

This is a big one!

We've reset the app.  The internals have been largely rewritten and fine-tuned.  And we have
simplified the commands:

```powerShell
# If you get stuck try the help system.
$ football-cli --help
$ football-cli competitions --help

# Returns all the supported competitions
$ football-cli competitions

# View today's fixtures
$ football-cli matches

# Include yesterdays results
$ football-cli matches --from -1

# Pick time ranges
$ football-cli matches --from '2023-09-01' --until '2023-09-02'

# Check upcoming matches (both return the same result)
$ football-cli matches --until 7
$ football-cli matches --until +7
```

All other commands have been retired.  At least for now.  

See our [roadmap](./road-map.md) for future plans.

## `0.6.0`

We've introduced a new view for our league tables.  The consice view shows the most important columns.
It is much thinner, which is helpful when you don't want to go full-screen.

The full view is still available behind a switch:

```
$ football-cli table pl --full-table
```

## `0.5.0`

Live results now highlight updated games.

## `0.4.1`

Following live results now looks much better.  Instead of outputting one result table after another;
we know overwrite the previous display.

## `0.4.0`

We now support different custom views for different competition tables.

You can select your favourite team.  They will be highlighted in competition tables.

Bug fix: Competition command now supports lower and mixed case competition codes.

## `0.3.0`

Follow live implemented :)
It's a big ugly :(

## `0.2.0`

Build and config instructions added to the readme file.
)
## `0.1.0`

Progress!  Although the app is flakey and unfinished; the core concepts have been proved.

In this release:

- You can view live competition tables
- You can view live match scores, in multiple competitions
- You can view all available competitions
