import Modal from '../../modal/Modal';
import ButtonClose from '../../ui/button_close/ButtonClose';
import ButtonX from '../../ui/button_x/ButtonX';
import Examples from '../../ui/examples/Examples';
import WordLvl from '../../ui/lvl/WordLvl';
import Source from '../../ui/source/Source';
import Transcription from '../../ui/transcription/Transcription';
import Translations from '../../ui/translations/Translations';
import Word from '../../ui/word/Word';
import styles from '../view_note/view_note_form.module.css';

function ViewNoteForm({isOpen, note, onClose, onOpenVideo}){

    const youtubeVideoId = note.source?.youtubeVideoId;
    const youtubeId = note.source?.youtubeId;
    const youtubeVideoTitle = note.source?.youtubeVideoTitle;
    const context = note.source?.context;
    const duration = note.source?.durationContext;    
    
    return(
        <Modal isOpen={isOpen}>
            <div className={styles.view__container}>
                <div className={styles.top_part}>
                    <div className={styles.block_lvl_close}>
                        <WordLvl lvl={note.lvl}/>
                        <ButtonX onClick={onClose}/>
                    </div>
                    <div className={styles.block_word_transcription}>
                        <Word word={note.word} size={36}/>
                        <Transcription word={note.word} transcription={note.transcription}/>
                    </div>
                    <div className={styles.block_translations}>
                        <Translations translations={note.translations} size={18}/>
                    </div>
                </div>
                <div className={styles.bottom_part}>
                    <Examples examples={note.examples} word={note.word}/>
                    {note.source && <Source 
                        youtubeVideoId={youtubeVideoId}
                        youtubeVideoTitle={youtubeVideoTitle} 
                        context={context}
                        duration={duration}
                        word={note.word}
                        onOpenVideo={() => onOpenVideo(youtubeVideoId, youtubeId, duration)}
                    />}
                </div>
            </div>
        </Modal>
    )
}

export default ViewNoteForm;