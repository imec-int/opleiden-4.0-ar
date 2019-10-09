#!/bin/sh

# Set custom git hooks folder
git config core.hooksPath .githooks

# Create git lfs hooks
git lfs install

# Move conflicting git lfs hooks to subdirectory
mv ./.githooks/post-checkout ./.githooks/post-checkout.d/git-lfs
mv ./.githooks/post-merge ./.githooks/post-merge.d/git-lfs

cp ./.githooks/multiple-git-hooks ./.githooks/post-checkout
cp ./.githooks/multiple-git-hooks ./.githooks/post-merge

# Setup Unity SmartMerge
