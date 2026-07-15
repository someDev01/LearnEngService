import { useEffect, useState } from "react";
import Modal from "../../modal/Modal";
import styles from '../training/training_modal.module.css';
import { noteApi } from "../../api/note";
import { toast } from "react-toastify";
import ButtonClose from "../../ui/button_close/ButtonClose";
import { useDispatch } from "react-redux";
import { closeModalTrainig } from "../../redux/slices/modalSlice";
import Source from "../../ui/source/Source";
import NoNotes from "../../ui/no_notes/NoNotes";
import ButtonX from "../../ui/button_x/ButtonX";
import { Volume2Icon } from "lucide-react";
import Translations from "../../ui/translations/Translations";
import Examples from "../../ui/examples/Examples";
import TrainingProgress from "../../ui/training_progress/TrainingProgress";
import TrainingPronuntion from "../../ui/training_pronuntion/TrainingPronuntion";
import Slots from "../../ui/slots/Slots";
import Tiles from "../../ui/tiles/Tiles";
import TrainingNextButton from "../../ui/training_next_button/TrainingNextButton";
import TrainingFinish from './../../ui/training_finish/TrainingFinish';

function TrainingModal({isOpen, trainingNotes=[]}) {

    const dispatch = useDispatch();

    const [notes, setNotes] = useState([]);
    const [trainingWords, setTrainingWords] = useState([]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [wordProgressIndex, setWordProgressIndex] = useState(0);
    const [tiles, setTiles] = useState([]);
    const [builded, setBuilded] = useState(false);
    const [isFinished, setIsFinished] = useState(false);
    const [wrongTouchCount, setWrongTouchCount] = useState(0);
    
    const currentIndexView = currentIndex + 1;
    const currentWord = trainingWords?.[currentIndex];
    const target = currentWord?.word ?? '';
    const targetTranslations = currentWord?.translations ?? [];

    useEffect(() => {
        if(trainingNotes.length > 0){
            setNotes(trainingNotes);
            return;
        }

        const getNotes = async () => {
            const response = await noteApi.getDictionary();

            if (response.success) {
                setNotes(response.data);
            } else {
                toast.error("Ошибка получения заметок");
                setNotes([]);
            }
        };

        getNotes();
    }, []);

    useEffect(() => {
        if (!notes.length) return;

        const selected = shuffle([...notes]
            .sort((a, b) => a.repetitionScore - b.repetitionScore)
            .slice(0, 10));

        setTrainingWords(selected);
        setCurrentIndex(0);
    }, [notes]);

    useEffect(() => {
        if(!currentWord) return;

        setTiles(shuffle(currentWord.word.split('').filter(c => c !== ' ').map((char, id) => ({char, id, isWrong: false}))));
    }, [currentWord, trainingWords])

    const onGoNext = () => {
        if (currentIndex === trainingWords.length - 1) {
            setIsFinished(true);
            return;
        }
        
        setCurrentIndex(prev => prev + 1);
        setWordProgressIndex(0);
        setBuilded(false);
    };

    function shuffle(array) {
        const a = [...array];
        for (let i = a.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [a[i], a[j]] = [a[j], a[i]];
        }
        return a;
    }

    const onTileClick = async(clickedTile) => {

        const needed = target[wordProgressIndex];
        if(clickedTile.char === needed){
            setTiles(prev => prev.filter(t => t.id !== clickedTile.id));

            let next = wordProgressIndex + 1;
            while(target[next] === ' ') next++;
            setWordProgressIndex(next);

            const isDone = next === target.length;
            if(isDone) {
                setBuilded(true);
                await noteApi.updateRepetitionScore(currentWord.id, isDone);
            }
        }
        else{
            setWrongTouchCount(prev => prev + 1);
            navigator.vibrate?.(50);
            setTiles(prev => prev.map(tile => tile.id === clickedTile.id ? {...tile, isWrong: true} : tile));

            setTimeout(() => {
                setTiles(prev => prev.map(tile => tile.id === clickedTile.id ? {...tile, isWrong: false} : tile));
            }, 300);
        }
    };

    const onRestartTraining = () => {
        setTrainingWords(prev => shuffle([...prev]));
        setCurrentIndex(0);
        setWordProgressIndex(0);
        setBuilded(false);
        setIsFinished(false);
        setWrongTouchCount(0);
    }

    const onClose = () => {
        dispatch(closeModalTrainig());
    };

    if (!isOpen) return null;

    return (
        <Modal isOpen={isOpen}>
            <section className={styles.training}>
                <header className={styles.header}>
                    {notes.length > 0 && !isFinished && (
                        <TrainingProgress currentIndex={currentIndex} trainingWords={trainingWords.length}/>
                    )}
                    <ButtonX onClick={onClose}/>
                </header>
                {notes.length > 0 ? (
                    <main className={styles.main}>
                    {!isFinished ? (
                        <div className={styles.content}>
                            <TrainingPronuntion target={target}/>
                            <section className={styles.word_builder}>
                                <Slots target={target} wordProgressIndex={wordProgressIndex} builded={builded}/>
                                <Tiles tiles={tiles} onTileClick={onTileClick}/>
                            </section>
                            {builded && (
                                <section className={styles.word_info}>
                                    <Translations translations={targetTranslations} size={14}/>
                                </section>
                            )}
                            {builded && !isFinished && (
                                <TrainingNextButton onGoNext={onGoNext}/>
                            )}
                        </div>
                        
                    ) : (
                        <TrainingFinish 
                            currentIndexView={currentIndexView} 
                            targetLength={trainingWords.length} 
                            wrongTouchCount={wrongTouchCount}
                            onRestartTraining={onRestartTraining}
                        />
                    )}
                    </main>) : (<NoNotes/>)
                }
            </section>
        </Modal>
    );
}

export default TrainingModal;