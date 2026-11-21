import Modal from '../../modal/Modal';
import ButtonX from '../../ui/button_x/ButtonX';
import Examples from '../../ui/examples/Examples';
import WordLvl from '../../ui/lvl/WordLvl';
import Transcription from '../../ui/transcription/Transcription';
import Translations from '../../ui/translations/Translations';
import Word from '../../ui/word/Word';
import styles from '../view_note/view_note_form.module.css';
import NoteSource from '../../features/dictionary/components/noteSource/NoteSource';

function ViewNoteForm({isOpen, note, onClose}){
    
    if(!note) return null;
    
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
                    {note?.source?.length > 0 && (
                        <NoteSource 
                            source={note.source}
                            word={note.word}
                        />
                    )}
                </div>
            </div>
        </Modal>
    )
}

export default ViewNoteForm;