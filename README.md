## Repo to track popular stackover flow tags.

_So I dont forget_

- Run Console app first, configure settings and create/populate tags table.
- Run RestApi to access api endpoints for such tags.

### OR

- Run `docker compose up` on `docker-compose.yml`
- Use `Refresh all tags` API method to refresh all tags.
- Query rest api.

## Media
![obraz](https://github.com/SebastianDrela2/StackOverFlowTagFetch/assets/107455395/a7893b15-0592-4163-b6a3-842b2a0f65aa)


### Open Api Doc
```
openapi: 3.0.0
info:
  title: Simplified Tag API
  description: API for managing simplified tags
  version: 1.0.0
servers:
  - url: /api
paths:
  /tags/data/{apiKey}:
    get:
      summary: Refresh all tags
      parameters:
        - in: path
          name: apiKey
          required: true
          schema:
            type: string
      responses:
        '200':
          description: Tags have been refreshed successfully
      security:
        - ApiKeyAuth: []
    post:
      summary: Create a new tag
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/Tag'
      responses:
        '204':
          description: Tag created successfully
      security:
        - ApiKeyAuth: []
  /tags/results:
    get:
      summary: Get sorted simplified tags
      parameters:
        - in: query
          name: page
          schema:
            type: integer
            minimum: 1
          description: Page number
        - in: query
          name: sort
          schema:
            type: string
            enum: [nameascending, namedescending, percentageascending, percentagedescending, countascending, countdescending]
          description: Sort option
      responses:
        '200':
          description: List of sorted simplified tags
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: '#/components/schemas/SimplifiedTag'
      security:
        - ApiKeyAuth: []
  /tags/{id}:
    get:
      summary: Get simplified tag by ID
      parameters:
        - in: path
          name: id
          required: true
          schema:
            type: integer
            minimum: 1
          description: Tag ID
      responses:
        '200':
          description: Simplified tag found
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/SimplifiedTag'
        '404':
          description: Simplified tag not found
      security:
        - ApiKeyAuth: []
    put:
      summary: Update a simplified tag
      parameters:
        - in: path
          name: id
          required: true
          schema:
            type: integer
            minimum: 1
          description: Tag ID
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/Tag'
      responses:
        '204':
          description: Tag updated successfully
      security:
        - ApiKeyAuth: []
    delete:
      summary: Delete a simplified tag
      parameters:
        - in: path
          name: id
          required: true
          schema:
            type: integer
            minimum: 1
          description: Tag ID
      responses:
        '204':
          description: Tag deleted successfully
      security:
        - ApiKeyAuth: []
components:
  schemas:
    SimplifiedTag:
      type: object
      properties:
        id:
          type: integer
          format: int64
        name:
          type: string
        percentage:
          type: number
        count:
          type: integer
    Tag:
      type: object
      properties:
        name:
          type: string
        percentage:
          type: number
        count:
          type: integer
securitySchemes:
  ApiKeyAuth:
    type: apiKey
    in: header
    name: X-API-Key

```
