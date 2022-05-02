FROM nikolaik/python-nodejs:python3.10-nodejs18-alpine
RUN apk add git gnupg
RUN git config --global --add safe.directory /workspaces/scripting-tanks
RUN npm install -g heroku
RUN pip install autopep8 pylint
