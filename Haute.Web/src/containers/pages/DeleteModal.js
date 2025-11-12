import React from 'react';
import {
  Row,
  Card,
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
} from 'reactstrap';
import IntlMessages from 'helpers/IntlMessages';
import { Colxx } from 'components/common/CustomBootstrap';

const DeleteModal = ({
  deleteMessage,
  isDeleteModalOpen,
  deletedClick,
  toggleModal,
}) => {
  return (
    <>
      {isDeleteModalOpen && (
        <Row>
          <Colxx xxs="12">
            <Card className="mb-4">
              <Modal isOpen={isDeleteModalOpen} toggle={toggleModal}>
                <ModalHeader>
                  <IntlMessages id="Confirmation" />
                </ModalHeader>
                <ModalBody>{deleteMessage}</ModalBody>
                <ModalFooter>
                  <Button color="primary" onClick={deletedClick}>
                    Yes
                  </Button>{' '}
                  <Button color="secondary" onClick={toggleModal}>
                    No
                  </Button>
                </ModalFooter>
              </Modal>
            </Card>
          </Colxx>
        </Row>
      )}
    </>
  );
};

export default DeleteModal;
