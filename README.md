## Repo to track popular stackover flow tags.

_So I dont forget_

- Run Console app first, configure settings and create/populate tags table.
- Run RestApi to access api endpoints for such tags.

## Media

![obraz](https://github.com/SebastianDrela2/StackOverFlowTagFetch/assets/107455395/cc47d3be-a467-4427-a544-3bef399e33ef)

### Open Api Doc
```
openapi: 3.0.0
info:
  title: Simplified Tag API
  description: API for managing simplified tags
  version: 1.0.0
servers:
  - url: http://localhost:5000
paths:
  /api/tags/results/{apiKey}:
    get:
      summary: Refresh All Tags
      description: Refresh all tags using the provided API key
      parameters:
        - in: path
          name: apiKey
          required: true
          description: API key for authentication
          schema:
            type: string
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: '#/components/schemas/SimplifiedTag'
        '204':
          description: No Content
  /api/tags:
    get:
      summary: Get All Simplified Tags
      description: Retrieve all simplified tags
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: '#/components/schemas/SimplifiedTag'
    post:
      summary: Create Simplified Tag
      description: Create a new simplified tag
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/Tag'
      responses:
        '204':
          description: No Content
    /{id}:
      get:
        summary: Get Simplified Tag by ID
        description: Retrieve a simplified tag by its ID
        parameters:
          - in: path
            name: id
            required: true
            description: ID of the simplified tag to retrieve
            schema:
              type: integer
        responses:
          '200':
            description: OK
            content:
              application/json:
                schema:
                  $ref: '#/components/schemas/SimplifiedTag'
          '404':
            description: Not Found
      put:
        summary: Update Simplified Tag
        description: Update an existing simplified tag
        parameters:
          - in: path
            name: id
            required: true
            description: ID of the simplified tag to update
            schema:
              type: integer
        requestBody:
          required: true
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/Tag'
        responses:
          '204':
            description: No Content
      delete:
        summary: Delete Simplified Tag
        description: Delete a simplified tag
        parameters:
          - in: path
            name: id
            required: true
            description: ID of the simplified tag to delete
            schema:
              type: integer
        responses:
          '204':
            description: No Content
components:
  schemas:
    SimplifiedTag:
      type: object
      properties:
        id:
          type: integer
        name:
          type: string
        percentage:
          type: number
      required:
        - id
        - name
        - percentage
    Tag:
      type: object
      properties:
        name:
          type: string
        percentage:
          type: number
      required:
        - name
        - percentage
```
