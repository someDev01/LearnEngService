import Modal from '../../modal/Modal';
import ButtonClose from '../../ui/button_close/ButtonClose';
import styles from '../video_player/video_player.module.css';
import YoutubePlayer from '../../ui/youtube_player/YoutubePlayer';
import Controls from '../../ui/controls/Controls';
import { useEffect, useState } from 'react';
import SubtitlesPanel from '../../ui/subtitles_panel/SubtitlesPanel';
import { useDispatch, useSelector } from 'react-redux';
import { closeModalWordPopup, openModalWordPopup, closeVideoModal } from '../../redux/slices/modalSlice';
import { parseSrt } from './../../utils/parse_subtitles/parseSrt';
import { videoPlayerApi } from '../../api/videoPlayer';
import { toast } from 'react-toastify';
import { formatTime } from '../../utils/format_time/formatTime';
import { noteApi } from '../../api/note';
import WordPopUp from '../popup/WordPopup';
import ButtonX from '../../ui/button_x/ButtonX';

function VideoPlayer({isOpen, videoId, selectedYoutubeId , startTime = null}){

    const dispatch = useDispatch();
    const isOpenPopup = useSelector(state => state.modal.isOpenModalWordPopup);    

    const [currentTime, setCurrentTime] = useState(0);
    const [player, setPlayer] = useState(null);
    const [data, setData] = useState(null);
    const [usersNotes, setUsersNotes] = useState([]);
    const [noteCreating, setNoteCreating] = useState(false);
    const [selectedWord, setSelectedWord] = useState(null); 

    const onHasWordInNotes = (word) => {
        if(!word) return false;
        return usersNotes.some(note => note.word.toLowerCase() === word.toLowerCase());
    }
    const getNoteByWord = (word) => {
        if(!word) return null;
        return usersNotes.find(note => note.word.toLowerCase() === word.toLowerCase());
    };

    const [subtitles, setSubtitles] = useState({
        en:[],
        ru:[]
    });
    const currentEn = subtitles.en.findLast(s => currentTime >= s.start && currentTime <= s.end) || null;
    const currentRu = subtitles.ru.findLast(s => currentTime >= s.start && currentTime <= s.end) || null;

    const [isShowedEn, setIsShowedEn] = useState(true);
    const [isShowedRu, setIsShowedRu] = useState(true);

    const onToggleEn = () => {setIsShowedEn(prev => !prev)};
    const onToggleRu = () => {setIsShowedRu(prev => !prev)};

    const closeModal = () => {
        setPlayer(null);
        dispatch(closeVideoModal())
    }    

    const closeWordPopup = () => {
        dispatch(closeModalWordPopup());
        setSelectedWord('');
    }

    const addWordWithContextToNote = async(text) => {
        setNoteCreating(true);

        const {hours, minutes, seconds} = formatTime(currentTime);

        const context = currentEn?.text;
           
        const response = await noteApi.createNoteWithContext(
            videoId,
            data.youtubeId,
            data.youtubeVideoTitle,
            hours,
            minutes,
            seconds,
            text,
            context
        );

        if(response.success){
            toast.success("Заметка создана")
            setUsersNotes(prev => [...prev, response.data]);
        }
        else{
            toast.error(response.error);
            setNoteCreating(false);
        }

        setNoteCreating(false);
    }

    const handleWordClick = (word) => {
        setSelectedWord(word);
        player?.pauseVideo?.();
        dispatch(openModalWordPopup());
    }

    const loadSubtitles = async(subtitles) => {

        const enSub = subtitles.find(s => s.language === 'en');
        const ruSub = subtitles.find(s => s.language === 'ru');

        if(!enSub && !ruSub) return;

        const enText = enSub?.value ?? "";
        const ruText = ruSub?.value ?? "";

        const en = parseSrt(enText);
        const ru = parseSrt(ruText);
        
        setSubtitles({en, ru});
    }

    useEffect(() => {        
        const fetchVideoPlayer = async(id) => {
                const [responseVideoPlayer, responseUsersNotes] = await Promise.all([
                    videoPlayerApi.getByVideoId(id),
                    noteApi.getDictionary()
                ]);

                if(responseVideoPlayer.success){
                    setData(responseVideoPlayer.data); 
                    await loadSubtitles(responseVideoPlayer.data.subtitles);

                    if(responseUsersNotes.success){
                        setUsersNotes(responseUsersNotes.data);
                    }
                    
                }
                else{
                    toast.error("Ошибка сервера");
                    dispatch(closeVideoModal());
                    setUsersNotes([]);
                    return;
                }
            }
            fetchVideoPlayer(videoId);
    }, [videoId, dispatch]);    
    
    return(
        <>
            <Modal isOpen={isOpen}>
                <div className={styles.video_container}>                
                    <div className={styles.top_part}>
                        <ButtonX onClick={closeModal}/>
                    </div>
                    <YoutubePlayer 
                        youtubeId={selectedYoutubeId}
                        onTimeChange={setCurrentTime}
                        onPlayerReady={setPlayer}
                        startTime={startTime}
                    />
                    <Controls
                        onToggleEn={onToggleEn}
                        onToggleRu={onToggleRu}
                        isShowedEn={isShowedEn}
                        isShowedRu={isShowedRu}
                    />
                    <SubtitlesPanel
                        isShowedEn={isShowedEn}
                        isShowedRu={isShowedRu}
                        textEn={currentEn?.text}
                        textRu={currentRu?.text}
                        onWordClick={handleWordClick}
                        onHasWordInNotes={onHasWordInNotes}
                    />        
                </div>
            </Modal>

            <WordPopUp 
                isOpen={isOpenPopup} 
                word={selectedWord} 
                onAddNoteWithContext={addWordWithContextToNote}
                onClose={closeWordPopup}
                noteCreating={noteCreating}
                setWord={setSelectedWord}
                onHasWordInNotes={onHasWordInNotes}
            />
        </>
    )
}

export default VideoPlayer;