#!/bin/bash

root=`git rev-parse --show-toplevel`

ln -s "${root}/scripts/pre-push" "${root}/.git/hooks/pre-push"
