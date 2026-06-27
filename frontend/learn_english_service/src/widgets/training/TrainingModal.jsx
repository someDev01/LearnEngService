import { useEffect, useState } from "react";
import Modal from "../../modal/Modal";
import styles from '../training/training_modal.module.css';
import { noteApi } from "../../api/note";
import { toast } from "react-toastify";
import ButtonClose from "../../ui/button_close/ButtonClose";
import { useDispatch } from "react-redux";
import { closeModalTrainig } from "../../redux/slices/modalSlice";
import Question from "../../ui/question/Question";
import Option from "../../ui/option/Option";
import TrainingFinish from "../../ui/training_finish/TrainingFinish";
import Source from "../../ui/source/Source";
import EllipsisText from "../../ui/ellipsis_text/EllipsisText";
import NoNotes from "../../ui/no_notes/NoNotes";
import QuestionTranslation from "../../ui/question_translation/QuestionTranslation";
import ButtonQuestionTranslation from "../../ui/button_question_translation/ButtonQuestionTranslation";
import ButtonX from "../../ui/button_x/ButtonX";

const timeoutNext = 1500;

function TrainingModal({ isOpen }) {

    const dispatch = useDispatch();

    const [questions, setQuestions] = useState([]);
    const [showTranslation, setShowTranslation] = useState(false);
    const [currentIndexQ, setCurrentIndexQ] = useState(0);
    const [selectedAns, setSelectedAns] = useState(null);
    const [isShowResult, setIsShowResult] = useState(false);
    const [notes, setNotes] = useState([]);
    const [isFinished, setIsFinished] = useState(false);
    const [stepMap, setStepMap] = useState({});
    const [countCorrect, setCountCorrect] = useState(0);    

    const currentQuestion = questions[currentIndexQ] || null;

    useEffect(() => {
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

        const sorted = [...notes]
            .sort((a, b) => a.repetitionScore - b.repetitionScore)
            .slice(0, 10);

        const selected = shuffle(sorted);

        const qs = selected
            .filter(note => {
                const {text} = getSentence(note);

                const normalizedText = text.trim().toLowerCase();
                const normalizedWord = note.word.trim().toLowerCase();

                return normalizedText !== normalizedWord;
            })
            .map(note => {

                const { text, translate } = getSentence(note);
                const examples = note.examples || [{ text: '', translate: ''}]
                const translation = examples.find(t => t.text === text).translate;
                const question = text.replaceAll(note.word.toLowerCase(), "___");

                return {
                    question,
                    translate,
                    step: 0,
                    options: generateOptions(note, notes),
                    correct: note.word,
                    noteId: note.id,
                };
        });

        setQuestions(qs);
        setCurrentIndexQ(0);
        setSelectedAns(null);
        setIsShowResult(false);
        setIsFinished(false);

    }, [notes]);

    const getSentence = (note) => {
        const examples = note.examples || [{text: '', translate: ''}];
        const step = stepMap[note.id] || 0;

        if (examples.length > 0) {
            const idx = (step - 1) % examples.length;
            const idxSafe = (idx < 0 ? examples.length + idx : idx) % examples.length;
            return {
                text: examples[idxSafe].text,
                translate: examples[idxSafe].translate,
                type: "example",
            };
        }

        return {
            text: `Translate "${note.word}"`,
            type: "fallback",
        };
    };

    const generateOptions = (note, notes) => {
        const options = new Set();

        options.add(note.word);

        const otherWords = notes
            .filter(n => n.id !== note.id)
            .map(n => n.word);

        const shuffled = shuffle(otherWords);

        for (let word of shuffled) {
            options.add(word);

            if (options.size === Math.min(4, notes.length)) {
                break;
            }
        }

        return shuffle([...options]);
    };

    const handleSelect = async (option) => {
        if (isShowResult) return;

        setSelectedAns(option);
        setIsShowResult(true);

        const isCorrect = option === currentQuestion.correct;
        const noteId = currentQuestion.noteId;

        if(isCorrect){
            setCountCorrect(prev => prev + 1);
        }

        setStepMap(prev => {
            const currentStep = prev[noteId] || 0;

            return {
                ...prev,
                [noteId]: isCorrect ? currentStep + 1 : currentStep
            };
        });

        await noteApi.updateRepetitionScore(noteId, isCorrect);

        setTimeout(() => {
            goNext();
        }, timeoutNext);
    };

    const restartTraining = () => {
        if (!notes.length) return;

        const sorted = [...notes]
            .sort((a, b) => a.repetitionScore - b.repetitionScore)
            .slice(0, 10);

        const selected = shuffle(sorted);

        const qs = selected.map(note => {
            const { text, translate } = getSentence(note);

            return {
                question: text.replaceAll(note.word, "___"),
                translate,
                options: generateOptions(note, notes),
                correct: note.word,
                noteId: note.id,
            };
        });

        setQuestions(qs);
        setCurrentIndexQ(0);
        setSelectedAns(null);
        setIsShowResult(false);
        setIsFinished(false);
        setCountCorrect(0);
        setStepMap({});
        setShowTranslation(false);
    };

    const goNext = () => {
        if (currentIndexQ === questions.length - 1) {
            setIsFinished(true);
            return;
        }

        setCurrentIndexQ(prev => prev + 1);
        setSelectedAns(null);
        setIsShowResult(false);
        setShowTranslation(false);
    };

    const getButtonStyle = (option) => {
        if (!isShowResult) return;

        const isCorrect = option === currentQuestion.correct;
        const isSelected = option === selectedAns;

        if (isCorrect) {
            return { backgroundColor: '#12be6e', color: 'black' };
        }

        if (isSelected && !isCorrect) {
            return { backgroundColor: '#c71515', color: 'black' };
        }

        return {};
    };

    function shuffle(array) {
        const a = [...array];
        for (let i = a.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [a[i], a[j]] = [a[j], a[i]];
        }
        return a;
    }

    const onClose = () => {
        dispatch(closeModalTrainig());
        restartTraining();
    };

    if (!isOpen) return null;

    return (
        <Modal isOpen={isOpen}>
            <div className={styles.training__container}>
                <div className={styles.training}>
                    <div className={styles.header_part}>
                        <div className={styles.steps}>
                            <p>{currentIndexQ + 1} / {questions.length}</p>
                        </div>
                        <div className={styles.close_part}>
                            <ButtonX onClick={onClose} />
                        </div>
                    </div>

                    {currentQuestion && !isFinished && (
                        <>
                            <Question question={currentQuestion.question} />
                            {showTranslation && currentQuestion.translate && (
                                <QuestionTranslation translation={currentQuestion.translate}/>
                            )}
                            {!showTranslation && (
                                <ButtonQuestionTranslation onClick={() => setShowTranslation(true)}/>
                            )}

                            <div className={styles.options}>
                                {currentQuestion.options.map((opt, i) => (
                                    <Option
                                        key={i}
                                        word={opt}
                                        onClick={() => handleSelect(opt)}
                                        isShowResult={isShowResult}
                                        style={getButtonStyle(opt)}

                                    />
                                ))}
                            </div>
                        </>
                    )}

                    {isFinished && (
                        <TrainingFinish countCorrect={countCorrect} questions={questions} onClick={restartTraining}/>
                    )}

                    {!currentQuestion && <NoNotes/>}

                </div>
            </div>
        </Modal>
    );
}

export default TrainingModal;