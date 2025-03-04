### Docker Netzwerk erstellen

``` shell
docker network create \
 --driver overlay \
 --scope swarm \
 --attachable \
 reverse-proxy
```
"--diver overlay", "--scope swarm" and "--attachable" are all needed for this network to work with stack deploy  

### Docker Volume erstellen

``` shell
docker volume create \
 --driver local \
 TheMover_traefik_certs
```


