import { useEffect, useState } from 'react';
import styles from '../dictionary/dictionary_page.module.css';
import TitleDictionary from '../../ui/title_dictionary/TitleDictionary';
import ButtonCreateNote from '../../ui/button_create_note/ButtonCreateNote';
import NotesSearch from '../../ui/notes_search/NotesSearch';
import Note from '../../ui/note/Note';
import { noteApi } from '../../api/note';
import { toast } from 'react-toastify';
import { useDispatch, useSelector } from 'react-redux';
import { closeModalEditNote, closeModalViewNote, openModalEditNote, openModalViewNote, openVideoModal } from '../../redux/slices/modalSlice';
import ViewNoteForm from '../../widgets/view_note/ViewNoteForm';
import { closeMenu } from '../../redux/slices/menuSlice';
import EditNoteForm from '../../widgets/edit_note/EditNoteForm';
import Progress from '../../widgets/progress/Progress';
import TrainingModal from '../../widgets/training/TrainingModal';
import VideoPlayer from '../../widgets/video_player/VideoPlayer';
import { convertToTimeSeconds } from './../../utils/convert_time/convertToTimeSeconds';
import { GetPageSize } from '../../utils/size/getPageSize';
import LoadMoreButton from '../../ui/button_load_more/LoadMoreButton';
import LoaderSearch from '../../ui/loader_search/LoaderSearch';
import { useSearch } from '../../hooks/search/useSearch';
import NoFound from '../../ui/nofound/NoFound';

const notesTimeout = 500; 
function DictionaryPage(){

    const dispatch = useDispatch();

    const isOpenView = useSelector(state => state.modal.isOpenModalViewNote);
    const isOpenEdit = useSelector(state => state.modal.isOpenModalEditNote);
    const isOpenVideo = useSelector(state => state.modal.isOpenVideoModal);
    const [videoId, setVideoId] = useState(null);
    const [selectedYoutubeId, setSelectedYoutubeId] = useState(null);
    const [videoTime, setVideoTime] = useState(0); 

    const [notes, setNotes] = useState([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const pageSize = GetPageSize();
    const [loadingNotes, setLoadingNotes] = useState(false);

    const [searchPage, setSearchPage] = useState(1);

    const [openedNote, setOpenedNote] = useState(null);
    const [editingNote, setEditingNote] = useState(null);
    const [selectedLanguage, setSelectedLanguage] = useState(null);
    const [loadingSave, setLoadingSave] = useState(false);

    const {query, setQuery, searchResults, setSearchResults, isLoading, setIsLoading, hasSearched, hasMore: searchHasMore, setHasMore: setSearchHasMore} 
    = useSearch({fetchFunction: (query, page) => noteApi.searchNotes(query, page, pageSize), extractData: (response) => response.data.data});

    const onOpenViewNote = (note) => {
        setOpenedNote(note);
        dispatch(openModalViewNote());
        dispatch(closeMenu());
    }

    const onOpenVideo = (videoId, youtubeId, duration) => {
        setVideoId(videoId);
        setSelectedYoutubeId(youtubeId);
        
        setVideoTime(convertToTimeSeconds(duration))
        dispatch(openVideoModal());
        dispatch(closeModalViewNote());
    };

    const onCloseViewNote = () => {dispatch(closeModalViewNote());}
    const onCloseEditNote = () => {dispatch(closeModalEditNote());}

    const onOpenEditNote = () => { 
        setEditingNote(null);
        dispatch(openModalEditNote());
    };

    const onOpenEditNoteWithParams = (note) => {
        setEditingNote(note);
        dispatch(openModalEditNote());
    };

    const onDeleteNote = (noteId) => {
        setNotes(prev => prev.filter(n => n.id !== noteId));
        setTotalCount(prev => prev - 1);
    };

    const onAddNote = (newNote) => {
        setNotes(prev => [newNote,...prev]);
        setTotalCount(prev => prev + 1);
    };

    const handleDelete = async(noteId) => {
        const response = await noteApi.deleteNote(noteId);

        if(response.success){
            onDeleteNote(noteId);
            toast.success("Заметка удалена");
        }

        else{
            toast.error(response.error);
        }
    }

    const handleCreate = async(word, translations, examples) => {
        setLoadingSave(true);
        const response = await noteApi.createNote(
            word,
            translations,
            examples
        );

        if(response.success){
            onAddNote(response.data);
            dispatch(closeModalEditNote());
            toast.success("Заметка создана");
        }
        else{
            toast.error(response.error);
        }

        setLoadingSave(false);
    }

    const handleUpdate = async(noteId, word, translations, examples) => {
        setLoadingSave(true);
        const response = await noteApi.updateNote(noteId, word, translations, examples);

        if(response.success){               
            setNotes(prev => prev.map(note => note.id === noteId ? response.data : note));
            dispatch(closeModalEditNote());
            toast.success("Заметка обнавлена");
        }
        else{
            toast.error(response.error);
        }

        setLoadingSave(false);
    }

    const loadMore = async() => {
        const nextPage = page + 1;
        setPage(nextPage);

        const response = await noteApi.getNotes(nextPage, pageSize);

        if(response.success){
            setNotes(prev => [
                ...prev,
                ...response.data.data
            ]);
        }

        else{
            toast.error("Ошибка получения заметок");
            setNotes([]);
        }

        setHasMore(response?.data.page < response?.data.totalPages);
    };

    const loadMoreSearch = async() => {
        const nextPage = searchPage + 1;
        setSearchPage(nextPage);

        const response = await noteApi.searchNotes(query, nextPage, pageSize);

        if(response.success){
            setSearchResults(prev => [
                ...prev, 
                ...response.data.data
            ]);

            setSearchHasMore(response.data.page < response.data.totalPages);
        }
        else{
            toast.error("Ошибка получения заметок");
            setSearchResults([]);
        }
    };

    const loadFirstPage = async() => {
        setLoadingNotes(true);
        setPage(1);

        const response = await noteApi.getNotes(1, pageSize);

        if(response.success){            
            setNotes(response.data.data);
            setTotalCount(response.data.totalCount);
        }
        else{
            toast.error("Ошибка получения заметок");
            setNotes([]);
        }
        setLoadingNotes(false);
        setHasMore(response?.data.page < response?.data.totalPages);
    }

    useEffect(() => {
        setSearchPage(1);
        setSearchResults([]);
        setSearchHasMore(false);
    }, [query.trim()]);

    useEffect(() => {
        loadFirstPage();
    }, [])

    const displayNotes = query.trim().length >= 2 ? searchResults: notes;

    return(
        <>
            <Progress userNotes={notes}/>
            <div className={styles.section_dictionary}>
                <div className={styles.header_dictionary}>
                    <div className={styles.top_part}>
                        <TitleDictionary count={`${notes.length}/${totalCount}`}/>
                        <ButtonCreateNote onClick={onOpenEditNote}/>
                    </div>
                    <div className={styles.bottom_part}>
                        <NotesSearch
                            query={query}
                            setQuery={setQuery}
                        />
                    </div>
                </div>
                {(loadingNotes || isLoading) && (
                    <div className={styles.wrapper_loader} style={{display: 'flex', justifyContent: 'center', alignItems: 'center', width: '100%'}}>
                        <LoaderSearch/>
                    </div>
                )}
                <div className={styles.list_notes}>
                    {!loadingNotes && !isLoading && displayNotes.map((note) => (
                        <Note 
                            key={note.id}  
                            word={note.word}
                            translations={note.translations}
                            transcription={note.transcription}
                            lvl={note.lvl}
                            createdAt={note.createdAt}
                            onOpenViewNote={() => {onOpenViewNote(note)}}
                            onOpenEditNote={() => {onOpenEditNoteWithParams(note)}}
                            onDeleteNote={() => {handleDelete(note.id)}}
                        />
                    ))}
                </div>
                {!loadingNotes && !isLoading && query.trim().length >= 2 && hasSearched && searchResults.length === 0 && (
                    <div className="" style={{display: 'flex', justifyContent:'center', alignItems:'center', width:'100%'}}>
                        <NoFound/>
                    </div>
                )}
                {!loadingNotes && !isLoading && query.trim().length < 2 && hasMore && (
                    <LoadMoreButton onClick={loadMore}/>
                )}
                {!loadingNotes && !isLoading && query.trim().length >= 2 && searchHasMore && (
                    <LoadMoreButton onClick={loadMoreSearch}/>
                )}
            </div>
            {isOpenView && <ViewNoteForm isOpen={isOpenView} note={openedNote} onClose={onCloseViewNote} onOpenVideo={onOpenVideo}/>}
            {isOpenEdit && 
                <EditNoteForm 
                    isOpen={isOpenEdit} 
                    onClose={onCloseEditNote} 
                    onCreateNote={handleCreate} 
                    onUpdateNote={handleUpdate}
                    note={editingNote} 
                    isEditing={!!editingNote}
                    loadingSave={loadingSave}
                />}
            {isOpenVideo && 
                <VideoPlayer isOpen={isOpenVideo} videoId={videoId} selectedYoutubeId={selectedYoutubeId} startTime={videoTime}/>}
        </>
    )
}

export default DictionaryPage;