" My own setup, by Kees
set nocompatible
set nobackup

" fix ruler
set number
set rnu

" Search more than one line
set incsearch

" set tabs correctly
set tabstop=4
set shiftwidth=4
set expandtab

" set colorscheme and syntax
syntax enable
set background=dark

" useful from http://vim.wikia.com/wiki/Best_Vim_Tips
:set ignorecase
:set smartcase
:syntax on

" tips from https://www.youtube.com/watch?v=aHm36-na4-4
nnoremap ; :

"spellcheck"
":set spell spelllang=en_us"

set noeb vb t_vb=
set vb t_vb=


call plug#begin('~/.vim/plugged')

Plug 'junegunn/fzf', { 'dir': '~/.fzf', 'do': './install --all' }

call plug#end()


