# yamoc

yamoc is a standalone HTTP mock tool.

## usage

1. make yamlfile

```setting.yaml
port: 8888

default:
  headers:
    Content-Type: text/plain

notfound:
  status: 404
  bodytext: '{ "message": "404 not found" }'
  headers:
    Content-Type: application/json

paths:
  # matching GET,POST /hello
  - path: /hello
    methods: get,post
    response:
      status: 200
      bodytext: hello
```

2. execute yamoc with yaml file

```bash
yamoc setting.yaml
```

3. access to localhost

```
yamoc version 1.0.0

   using `setting.yaml`

   HTTP started   http://localhost:8888/
   Ctrl + C to stop

001: --------------------------------------------------------------------------------
001: 2020/02/22 14:28:16
001: ===== Request =====
001: POST /hello
001: [Headers] Origin: chrome-extension://aejoelaoggembcahagimdiliamlcdmfm, Sec-Fetch-Site: cross-site, Sec-Fetch-Mode: cors, Connection: keep-alive, Content-Length: 0, Content-Type: application/json, Accept: */*, Accept-Encoding: gzip, deflate, br, Accept-Language: ja,en-US;q=0.9,en;q=0.8, Host: localhost:8888, User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36,
001: ===== Response =====
001: [StatusCode] 200
001: [Headers   ] Content-Type: text/plain,
001: [Body      ] hello
001: 2020/02/22 14:28:16
```
